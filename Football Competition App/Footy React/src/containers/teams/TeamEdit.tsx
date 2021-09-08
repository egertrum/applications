import { useContext, useEffect, useState } from 'react';
import { Redirect, useParams } from 'react-router-dom';
import Alert, { EAlertClass } from '../../components/Alert';
import Loader from "../../components/Loader";
import { AppContext } from '../../context/AppContext';
import { ICountry } from '../../dto/ICountry';
import { ITeam } from '../../dto/ITeam';
import { BaseService } from '../../services/base-service';
import { EPageStatus } from '../../types/EPageStatus';
import { IFormProps } from '../../types/IFormProps';
import { IRouteId } from '../../types/IRouteId';

const validation = {
    error: "",
    country: "",
    name: "",
    amount: ""
}

const TeamEditView = (props: IFormProps<ITeam>) => {

    const [countries, setCountries] = useState([] as ICountry[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [alertMessage, setAlertMessage] = useState(validation);
    const [added, setSubmitStatus] = useState({ submitStatus: false });
    const appState = useContext(AppContext);

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

        if (props.values.countryId === "-1") {
            setAlertMessage(prevState => ({
                ...prevState,
                country: "Country field is required."
            }));
            formIsValid = false;
        }

        if (!props.values.name) {
            setAlertMessage(prevState => ({
                ...prevState,
                name: "Name field is required."
            }));
            formIsValid = false;
        }

        if(props.values.playersAmount! as unknown as number % 1 !== 0) {
            setAlertMessage(prevState => ({
                ...prevState,
                amount: "Amount of players can not be a float number."
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

        if (props.values.country!.id !== props.values.countryId) {
            props.values.country = undefined;
        }
        if(!props.values.playersAmount) {
            props.values.playersAmount = "0";
        }

        let response = await BaseService.put<ITeam>('/Teams/' + props.values.id, props.values, appState.token!);
        if (response.ok) {
            setSubmitStatus({ submitStatus: true });
        } else {
            setAlertMessage(prevState => ({ ...prevState, error: "Error code: " + response.statusCode.toString() }));
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    if (props.values) {
        return (
            <>
                { appState.token === null ? <Redirect to="/identity/login" /> : null}
                { added.submitStatus === true ? <Redirect to="/teams" /> : null}

                <h4 className="text-center">{props.values.name}</h4>
                <Alert show={alertMessage.error !== ''} message={alertMessage.error} alertClass={EAlertClass.Danger} />
                <hr />
                <div className="row">
                    <div className="col-md-4">
                        <form method="post">
                            <p className="redStar">* Field is required</p>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="Team_CountryId">Country</label> *
                                </div>
                                <select value={props.values.countryId} onChange={(e) => props.handleChange(e.target)} className="form-control" data-val="true" id="Team_CountryId" name="Team.CountryId">
                                    <option value="-1">---Please select---</option>
                                    {countries.map(country => {
                                        return <option key={country.id} value={country!.id!}>{country.name}</option>
                                    })}
                                </select>
                                {alertMessage.country !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.country}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="Team_Name">Name</label> *
                                </div>
                                <input value={props.values.name} onChange={(e) => props.handleChange(e.target)} className="form-control" type="text" data-val="true" id="Team_Name" maxLength={256} name="Team.Name" />
                                {alertMessage.name !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.name}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="Team_PlayersAmount">Amount of players</label> *
                                </div>
                                <input value={props.values.playersAmount !== null ? props.values.playersAmount as unknown as number : ""} onChange={(e) => props.handleChange(e.target)} className="form-control" type="number" data-val="true" id="Team_PlayersAmount" name="Team.PlayersAmount" />
                                {alertMessage.amount !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.amount}</span> </> : null}
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

const TeamEdit = () => {
    const [formValues, setFormValues] = useState({} as ITeam);
    const appState = useContext(AppContext);
    const { id } = useParams() as IRouteId;

    const loadData = async () => {
        let result = await BaseService.get<ITeam>('/Teams/' + id, appState.token!);
        if (result.ok && result.data) {
            setFormValues(result.data);
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const handleChange = (target: HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement) => {

        if (target.id === 'Team_CountryId') {
            setFormValues({ ...formValues, countryId: target.value });
            return;
        }
        if (target.id === 'Team_Name') {
            setFormValues({ ...formValues, name: target.value });
            return;
        }
        if (target.id === 'Team_PlayersAmount') {
            setFormValues({ ...formValues, playersAmount: target.value });
            return;
        }
    }
    return <TeamEditView values={formValues} handleChange={handleChange} />
};

export default TeamEdit;