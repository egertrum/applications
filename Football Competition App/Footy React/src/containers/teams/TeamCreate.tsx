import { useContext, useEffect, useState } from 'react';
import { Redirect } from 'react-router-dom';
import Alert, { EAlertClass } from '../../components/Alert';
import Loader from '../../components/Loader';
import { AppContext } from '../../context/AppContext';
import { ICountry } from '../../dto/ICountry';
import { ITeam } from '../../dto/ITeam';
import { BaseService } from '../../services/base-service';
import { EPageStatus } from '../../types/EPageStatus';
import { IFormProps } from '../../types/IFormProps';

const initialFormValues: ITeam = {
    countryId: '',
    appUserId: '00000000-0000-0000-0000-000000000000',
    name: '',
    playersAmount: ""
};

const validation = {
    error: "",
    country: "",
    name: "",
    amount: ""
}


const TeamCreateView = (props: IFormProps<ITeam>) => {

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

        if(props.values.countryId === "-1" || !props.values.countryId) {
            setAlertMessage(prevState => ({
                ...prevState,
                country: "Country field is required."
            }));
            formIsValid = false;
        }

        if(!props.values.name) {
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

        let response = await BaseService.post<ITeam>('/Teams', props.values, appState.token!);
        if (response.ok) {
            setSubmitStatus({ submitStatus: true });
        } else {
            setAlertMessage(prevState => ({...prevState, error: "Error code: " + response.statusCode.toString()}));
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    return (
        <>
            { appState.token === null ? <Redirect to="/identity/login" /> : null}
            { added.submitStatus === true ? <Redirect to="/teams" /> : null}

            <h4 className="text-center">Create a new team</h4>
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
                            <input type="submit" onClick={(e) => submitClicked(e.nativeEvent)} value="Create" className="btn btn-primary" />
                        </div>
                    </form>
                </div>
            </div>
            <Loader {...pageStatus} />
        </>
    );
}

const TeamCreate = () => {

    const [formValues, setFormValues] = useState(initialFormValues);

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
    return <TeamCreateView values={formValues} handleChange={handleChange} />
};

export default TeamCreate;