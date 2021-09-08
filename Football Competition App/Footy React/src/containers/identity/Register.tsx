import { useContext, useEffect, useState } from 'react';
import { Redirect } from 'react-router-dom';
import Alert, { EAlertClass } from '../../components/Alert';
import Loader from '../../components/Loader';
import { AppContext } from '../../context/AppContext';
import { ICountry } from '../../dto/ICountry';
import { IRegister } from '../../dto/IRegister';
import { BaseService } from '../../services/base-service';
import { IdentityService } from '../../services/identity-service';
import { EPageStatus } from '../../types/EPageStatus';
import { IFormProps } from '../../types/IFormProps';

const initialFormValues: IRegister = {
    countryId: '',
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    birthDate: '',
    gender: '',
    identificationCode: ''
};

const validation = {
    error: "",
    email: "",
    password: "",
    confirmPassword: "",
    identificationCode: "",
    dateOfBirth: "",
    country: "",
    firstname: "",
    lastname: ""
}

const RegisterView = (props: IFormProps<IRegister>) => {

    const [countries, setCountries] = useState([] as ICountry[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [registered, setRegisterStatus] = useState({ submitStatus: false });
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
        
        const emailRe = /^(([^<>()[\].,;:\s@"]+(\.[^<>()[\],;:\s@"]+)*)|(".+"))@(([^<>()[\],;:\s@"]+\.)+[^<>()[\],;:\s@"]{2,})$/i;
        const passwordRe = /^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{8,}$/i;
        let formIsValid = true;
        setAlertMessage(validation);

        if(!emailRe.test(props.values.email!)){
            setAlertMessage(prevState => ({
                ...prevState,
                email: "Email is not valid!"
            }));
            formIsValid = false;
        }

        if(!passwordRe.test(props.values.password)) {
            setAlertMessage(prevState => ({
                ...prevState,
                password: "Password requirements: Minimum 8 characters, at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character:"
            }));
            formIsValid = false;
        }

        if(props.values.password !== props.values.confirmPassword) {
            setAlertMessage(prevState => ({
                ...prevState,
                password: "Password and confirm password are not the same!"
            }));
            formIsValid = false;
        }

        if(!props.values.firstName) {
            setAlertMessage(prevState => ({
                ...prevState,
                firstname: "First name field is required."
            }));
            formIsValid = false;
        }

        if(!props.values.lastName) {
            setAlertMessage(prevState => ({
                ...prevState,
                lastname: "Last name field is required."
            }));
            formIsValid = false;
        }

        if(!props.values.identificationCode) {
            setAlertMessage(prevState => ({
                ...prevState,
                identificationCode: "Identification code field is required."
            }));
            formIsValid = false;
        }

        if(props.values.countryId === "-1" || !props.values.countryId) {
            setAlertMessage(prevState => ({
                ...prevState,
                country: "Country field is required."
            }));
            formIsValid = false;
        }

        if(!props.values.birthDate) {
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

        let response = await IdentityService.Register('/Account/Register', props.values);
        if (response.ok) {
            setRegisterStatus({ submitStatus: true });
            appState.setAuthInfo(response.data!.token, response.data!.firstname, response.data!.lastname, response.data!.role);
        } else {
            setAlertMessage(prevState => ({...prevState, error: "Error code: " + response.statusCode.toString()}));
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps


    if (registered.submitStatus === true) {
        return <> <Redirect to="/" /> </>
    }
    if (countries) {
        return (
            <>
                <h1>Register</h1>
                <div className="row">
                    <div className="col-md-4">
                        <form method="post" id="register-form">
                            <h4>Create a new account.</h4>
                            <Alert show={alertMessage.error !== ''} message={alertMessage.error} alertClass={EAlertClass.Danger} />
                            <hr />
                            <div className="form-group">
                                <label htmlFor="Input_Email">Email</label>
                                <input className="form-control" value={props.values.email ?? ""} onChange={(e) => props.handleChange(e.target)} type="email" id="Input_Email" name="Input.Email" />
                                {alertMessage.email !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.email}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <label htmlFor="Input_Password">Password</label>
                                <input className="form-control" value={props.values.password} onChange={(e) => props.handleChange(e.target)} type="password" id="Input_Password" maxLength={100} name="Input.Password" />
                                {alertMessage.password !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.password}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <label htmlFor="Input_ConfirmPassword">Confirm Password</label>
                                <input className="form-control" value={props.values.confirmPassword} onChange={(e) => props.handleChange(e.target)} type="password" id="Input_ConfirmPassword" name="Input.ConfirmPassword" />
                                {alertMessage.confirmPassword !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.confirmPassword}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <label className="control-label" htmlFor="Input_CountryId">Country of birth</label>
                                <select className="form-control" value={props.values.countryId} onChange={(e) => props.handleChange(e.target)} id="Input_CountryId" name="Input.CountryId">
                                    <option value="-1">---Please select---</option>
                                    {countries.map(country => {
                                        return <option key={country.id} value={country!.id!}>{country.name}</option>
                                    })}
                                </select>
                                {alertMessage.country !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.country}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <label className="control-label" htmlFor="Input_BirthDate">Date of birth</label>
                                <input className="form-control" value={props.values.birthDate !== '' ? dateformat(props.values.birthDate, "isoDate") : ''} onChange={(e) => props.handleChange(e.target)} type="date" id="Input_BirthDate" name="Input.BirthDate" />
                                {alertMessage.dateOfBirth !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.dateOfBirth}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <label htmlFor="Input_FirstName">First name</label>
                                <input className="form-control" value={props.values.firstName} onChange={(e) => props.handleChange(e.target)} type="text" id="Input_FirstName" maxLength={128} name="Input.FirstName" />
                                {alertMessage.firstname !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.firstname}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <label htmlFor="Input_LastName">Last name</label>
                                <input className="form-control" value={props.values.lastName} onChange={(e) => props.handleChange(e.target)} type="text" id="Input_LastName" maxLength={128} name="Input.LastName" />
                                {alertMessage.lastname !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.lastname}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <label className="control-label" htmlFor="Input_IdentificationCode">Identification code</label>
                                <input className="form-control" value={props.values.identificationCode} onChange={(e) => props.handleChange(e.target)} type="text" id="Input_IdentificationCode" maxLength={128} name="Input.IdentificationCode" />
                                {alertMessage.identificationCode !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.identificationCode}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <label className="control-label" htmlFor="Input_Gender">Gender</label>
                                <select className="custom-select mr-sm-2" value={props.values.gender} onChange={(e) => props.handleChange(e.target)} id="Input_Gender" name="Input.Gender">
                                    <option value="">Choose...</option>
                                    <option value="Man">Man</option>
                                    <option value="Woman">Woman</option>
                                </select>
                            </div>
                            <button type="submit" onClick={(e) => submitClicked(e.nativeEvent)} className="btn btn-primary">Register</button>

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

const Register = () => {

    const [formValues, setFormValues] = useState(initialFormValues);

    const handleChange = (target: HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement) => {

        if (target.id === 'Input_Email') {
            setFormValues({ ...formValues, email: target.value });
            return;
        }
        if (target.id === 'Input_Password') {
            setFormValues({ ...formValues, password: target.value });
            return;
        }
        if (target.id === 'Input_ConfirmPassword') {
            setFormValues({ ...formValues, confirmPassword: target.value });
            return;
        }
        if (target.id === 'Input_CountryId') {
            setFormValues({ ...formValues, countryId: target.value });
            return;
        }
        if (target.id === 'Input_FirstName') {
            setFormValues({ ...formValues, firstName: target.value });
            return;
        }
        if (target.id === 'Input_LastName') {
            setFormValues({ ...formValues, lastName: target.value });
            return;
        }
        if (target.id === 'Input_IdentificationCode') {
            setFormValues({ ...formValues, identificationCode: target.value });
            return;
        }
        if (target.id === 'Input_BirthDate') {
            setFormValues({ ...formValues, birthDate: target.value as unknown as Date });
            return;
        }
        if (target.id === 'Input_Gender') {
            setFormValues({ ...formValues, gender: target.value });
            return;
        }
    }

    return <RegisterView values={formValues} handleChange={handleChange} />
};

export default Register;