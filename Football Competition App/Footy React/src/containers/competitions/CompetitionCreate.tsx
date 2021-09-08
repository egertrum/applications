import React, { useContext, useEffect, useState } from 'react';
import { Redirect } from 'react-router-dom';
import Alert, { EAlertClass } from '../../components/Alert';
import Loader from '../../components/Loader';
import { AppContext } from '../../context/AppContext';
import { ICompetition } from '../../dto/ICompetition';
import { ICountry } from '../../dto/ICountry';
import { BaseService } from '../../services/base-service';
import { EPageStatus } from '../../types/EPageStatus';
import { IFormProps } from '../../types/IFormProps';
import information from '../../wwwroot/images/information.png'

const initialFormValues: ICompetition = {
    countryId: '',
    name: '',
    organiser: '',
    startDate: null,
    endDate: null,
    comment: null
};

const validation = {
    error: "",
    name: "",
    country: "",
    organiser: "",
    startDate: ""
}

const CompetitionCreateView = (props: IFormProps<ICompetition>) => {

    const [countries, setCountries] = useState([] as ICountry[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [alertMessage, setAlertMessage] = useState(validation);
    const [added, setSubmitStatus] = useState({ submitStatus: false });
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
                name: "Competition name field is required."
            }));
            formIsValid = false;
        }

        if(!props.values.organiser) {
            setAlertMessage(prevState => ({
                ...prevState,
                organiser: "Organiser field is required."
            }));
            formIsValid = false;
        }

        if(!props.values.startDate) {
            setAlertMessage(prevState => ({
                ...prevState,
                startDate: "Start date field is required."
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

        let response = await BaseService.post<ICompetition>('/Competitions', props.values, appState.token!);
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
            { added.submitStatus === true ? <Redirect to="/" /> : null}

            <h4 className="text-center">New competition</h4>
            <Alert show={alertMessage.error !== ''} message={alertMessage.error} alertClass={EAlertClass.Danger} />
            <hr/>
            <div className="row">
                <div className="col-md-4">
                    <div className="card shadow">
                        <a href="#collapseCardExample" className="d-block card-header py-3" data-toggle="collapse" aria-expanded="true" aria-controls="collapseCardExample">
                            <h6 className="m-0 font-weight-light text-dark">
                                <img className="very-small-icon" src={information} alt=""/>
                                    About organising
                            </h6>
                        </a>
                            <div className="collapse" id="collapseCardExample">
                                <div className="card-body">
                                <div>
                                    <strong>-</strong>If you are the organiser then just put your fullname into organiser
                                </div>
                                <div>
                                    <strong>-</strong>If the competition is going to take place just for one day you can leave the end date empty
                                </div>
                                 <div>
                                    <strong>-</strong>Everything else please add to comments: the exact place, entry fees, maximum amout on players per team etc.
                                </div>
                                </div>
                            </div>
                    </div>
                </div>
            </div>
            <hr/>
            <p className="redStar">* Field is required</p>
            <div className="row">
                <div className="col-md-4">
                    <form>
                        <div className="form-group">
                            <div>
                                <label className="control-label">In which country is it taking place?</label> *
                            </div>
                            <select value={props.values.countryId} onChange={(e) => props.handleChange(e.target)} className="form-control" data-val="true" data-val-required="The In which country is it taking place? field is required." id="Competition_CountryId" name="Competition.CountryId">
                                <option>---Please select---</option>
                                {countries.map(country => {
                                    return <option key={country.id} value={country!.id!}>{country.name}</option> 
                                })}
                            </select>
                            {alertMessage.country !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.country}</span> </> : null}
                        </div>
                        <div className="form-group">
                            <div>
                                <label className="control-label" htmlFor="Competition_Name">Competition name</label> *
                            </div>
                            <input value={props.values.name} onChange={(e) => props.handleChange(e.target)} className="form-control" type="text" id="Competition_Name"/>
                            {alertMessage.name !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.name}</span> </> : null}
                        </div>
                        <div className="form-group">
                            <div>
                                <label className="control-label" htmlFor="Competition_Organiser">Organiser</label> *
                            </div>
                            <input value={props.values.organiser} onChange={(e) => props.handleChange(e.target)} className="form-control" type="text" data-val="true" data-val-maxlength="The field Organiser must be a string with a maximum length of 64." data-val-maxlength-max="64" data-val-required="The Organiser field is required." id="Competition_Organiser" maxLength={64} name="Competition.Organiser"/>
                            {alertMessage.organiser !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.organiser}</span> </> : null}
                        </div>
                        <div className="form-group">
                            <div>
                                <label className="control-label" htmlFor="Competition_StartDate">Start date</label> *
                            </div>
                            <input value={props.values.startDate !== null ? dateformat(props.values.startDate, "isoDate") : ''} onChange={(e) => props.handleChange(e.target)} className="form-control" type="date" data-val="true" data-val-required="The Start date field is required." id="Competition_StartDate" name="Competition.StartDate"/>
                            {alertMessage.startDate !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.startDate}</span> </> : null}
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Competition_EndDate">End date</label>
                            <input value={props.values.endDate !== null ? dateformat(props.values.endDate, "isoDate") : ''} onChange={(e) => props.handleChange(e.target)} className="form-control" type="date" id="Competition_EndDate" name="Competition.EndDate"/>
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Competition_Comment">Comment</label>
                            <textarea value={props.values.comment ?? ""} onChange={(e) => props.handleChange(e.target)} className="form-control" data-val="true" data-val-maxlength="The field Comment must be a string with a maximum length of 4064." data-val-maxlength-max="4064" id="Competition_Comment" maxLength={4064} name="Competition.Comment">
                            </textarea>
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

const CompetitionCreate = () => {

    const [formValues, setFormValues] = useState(initialFormValues);

    const handleChange = (target: HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement) => {

        if (target.id === 'Competition_CountryId') {
            setFormValues({ ...formValues, countryId: target.value });
            return;
        }
        if (target.id === 'Competition_Name') {
            setFormValues({ ...formValues, name: target.value });
            return;
        }
        if (target.id === 'Competition_Organiser') {
            setFormValues({ ...formValues, organiser: target.value });
            return;
        }
        if (target.id === 'Competition_StartDate') {
            setFormValues({ ...formValues, startDate: target.value as unknown as Date });
            return;
        }
        if (target.id === 'Competition_EndDate') {
            setFormValues({ ...formValues, endDate: target.value as unknown as Date });
            return;
        }
        if (target.id === 'Competition_Comment') {
            setFormValues({ ...formValues, comment: target.value });
            return;
        }
    }
    return <CompetitionCreateView values={formValues} handleChange={handleChange} />
};

export default CompetitionCreate;