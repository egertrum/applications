import { useContext, useEffect, useState } from 'react';
import { Redirect, useParams } from 'react-router-dom';
import Alert, { EAlertClass } from '../../components/Alert';
import Loader from '../../components/Loader';
import { AppContext } from '../../context/AppContext';
import { ICountry } from '../../dto/ICountry';
import { IPerson } from '../../dto/IPerson';
import { BaseService } from '../../services/base-service';
import { EPageStatus } from '../../types/EPageStatus';
import { IFormProps } from '../../types/IFormProps';
import { IRouteId } from '../../types/IRouteId';

const validation = {
    error: "",
    identificationCode: "",
    dateOfBirth: "",
    country: "",
    firstname: "",
    lastname: ""
}

const PersonEditView = (props: IFormProps<IPerson>) => {

    const [countries, setCountries] = useState([] as ICountry[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [added, setSubmitStatus] = useState({ submitStatus: false });
    const [alertMessage, setAlertMessage] = useState(validation);
    const appState = useContext(AppContext);
    var dateformat = require("dateformat");

    const loadData = async () => {
        let result = await BaseService.getAll<ICountry>('/Countries');

        if (result.ok && result.data) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            setCountries(result.data);
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
        }
    }

    const handleValidation = () => {
        let formIsValid = true;
        setAlertMessage(validation);

        if (!props.values.firstName) {
            setAlertMessage(prevState => ({
                ...prevState,
                firstname: "First name field is required."
            }));
            formIsValid = false;
        }

        if (!props.values.lastName) {
            setAlertMessage(prevState => ({
                ...prevState,
                lastname: "Last name field is required."
            }));
            formIsValid = false;
        }

        if (!props.values.identificationCode) {
            setAlertMessage(prevState => ({
                ...prevState,
                identificationCode: "Identification code field is required."
            }));
            formIsValid = false;
        }

        if (props.values.countryId === "-1" || !props.values.countryId) {
            setAlertMessage(prevState => ({
                ...prevState,
                country: "Country field is required."
            }));
            formIsValid = false;
        }

        if (!props.values.birthDate) {
            setAlertMessage(prevState => ({
                ...prevState,
                dateOfBirth: "Birthdate field is required."
            }));
            formIsValid = false;
        }

        return formIsValid;
    }

    const submitClicked = async (e: Event) => {
        e.preventDefault();

        if (!handleValidation()) {
            return;
        };

        let response = await BaseService.put<IPerson>('/Persons/' + props.values.id, props.values, appState.token!);
        if (response.ok) {
            setSubmitStatus({ submitStatus: true });
        } else {
            setAlertMessage(prevState => ({ ...prevState, error: "Error code: " + response.statusCode.toString() }));
        }
    }

    useEffect(() => {
        loadData();
    }, []);

    if (props.values && props.values.country) {
        return (
            <>
                { appState.token === null ? <Redirect to="/identity/login" /> : null}
                { added.submitStatus === true ? <Redirect to="/persons" /> : null}

                <h4 className="text-center">{props.values.firstName} {props.values.lastName}</h4>
                <Alert show={alertMessage.error !== ''} message={alertMessage.error} alertClass={EAlertClass.Danger} />
                <hr />
                <div className="row">
                    <div className="col-md-4">
                        <form method="post">
                            <p className="redStar">* Field is required</p>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="Person_CountryId">Birth country</label> *
                                </div>
                                <select className="form-control" value={props.values.countryId} onChange={(e) => props.handleChange(e.target)} id="Person_CountryId" name="Person.CountryId">
                                    <option value="-1">---Please select---</option>
                                    {countries.map(country => {
                                        return <option key={country.id} value={country!.id!}>{country.name}</option>
                                    })}
                                </select>
                                {alertMessage.country !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.country}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="Person_FirstName">First name</label> *
                            </div>
                                <input className="form-control" value={props.values.firstName} onChange={(e) => props.handleChange(e.target)} type="text" id="Person_FirstName" maxLength={128} name="Person.FirstName" />
                                {alertMessage.firstname !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.firstname}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="Person_LastName">Last name</label> *
                            </div>
                                <input className="form-control" value={props.values.lastName} onChange={(e) => props.handleChange(e.target)} type="text" id="Person_LastName" maxLength={128} name="Person.LastName" />
                                {alertMessage.lastname !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.lastname}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="Person_IdentificationCode">Identification code</label> *
                            </div>
                                <input className="form-control" value={props.values.identificationCode} onChange={(e) => props.handleChange(e.target)} type="text" id="Person_IdentificationCode" name="Person.IdentificationCode" />
                                {alertMessage.identificationCode !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.identificationCode}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="Person_BirthDate">Date of birth</label> *
                            </div>
                                <input className="form-control" value={props.values.birthDate !== null ? dateformat(props.values.birthDate, "isoDate") : ''} onChange={(e) => props.handleChange(e.target)} type="date" id="Person_BirthDate" name="Person.BirthDate" />
                                {alertMessage.dateOfBirth !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.dateOfBirth}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <label className="mr-sm-2" htmlFor="Person_Gender">Gender</label>
                                <select className="custom-select mr-sm-2" value={props.values.gender} onChange={(e) => props.handleChange(e.target)} id="Person_Gender" name="Person.Gender">
                                    <option value="">Choose...</option>
                                    <option value="Man">Man</option>
                                    <option value="Woman">Woman</option>
                                </select>
                            </div>
                            <div className="form-group">
                                <input type="submit" onClick={(e) => submitClicked(e.nativeEvent)} value="Edit" className="btn btn-primary" />
                            </div>
                        </form>
                    </div>
                </div>
            </>
        );
    }
    return (
        <Loader {...pageStatus} />
    );
}

const PersonEdit = () => {

    const [formValues, setFormValues] = useState({} as IPerson);
    const appState = useContext(AppContext);
    const { id } = useParams() as IRouteId;

    const loadData = async () => {
        let result = await BaseService.get<IPerson>('/Persons/' + id, appState.token!);
        if (result.ok && result.data) {
            setFormValues(result.data);
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const handleChange = (target: HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement) => {

        if (target.id === 'Person_CountryId') {
            setFormValues({ ...formValues, countryId: target.value });
            return;
        }
        if (target.id === 'Person_FirstName') {
            setFormValues({ ...formValues, firstName: target.value });
            return;
        }
        if (target.id === 'Person_LastName') {
            setFormValues({ ...formValues, lastName: target.value });
            return;
        }
        if (target.id === 'Person_IdentificationCode') {
            setFormValues({ ...formValues, identificationCode: target.value });
            return;
        }
        if (target.id === 'Person_BirthDate') {
            setFormValues({ ...formValues, birthDate: target.value as unknown as Date });
            return;
        }
        if (target.id === 'Person_Gender') {
            setFormValues({ ...formValues, gender: target.value });
            return;
        }
    }
    return <PersonEditView values={formValues} handleChange={handleChange} />
};

export default PersonEdit;