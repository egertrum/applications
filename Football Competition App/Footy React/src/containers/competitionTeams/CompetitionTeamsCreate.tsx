import { useContext, useEffect, useState } from 'react';
import { Redirect, useParams } from 'react-router-dom';
import Alert, { EAlertClass } from '../../components/Alert';
import Loader from '../../components/Loader';
import { AppContext } from '../../context/AppContext';
import { ICompetition } from '../../dto/ICompetition';
import { ICompetitionSearch } from '../../dto/ICompetitionSearch';
import { ICompetitionTeam } from '../../dto/ICompetitionTeam';
import { ICountry } from '../../dto/ICountry';
import { ITeam } from '../../dto/ITeam';
import { BaseService } from '../../services/base-service';
import { EPageStatus } from '../../types/EPageStatus';
import { IFormTwoEntityProps } from '../../types/IFormTwoEnitityProps';
import { IRouteId } from '../../types/IRouteId';

const initialFormValues: ICompetitionTeam = {
    teamId: '',
    competitionId: ''
};

const initialSearchValues: ICompetitionSearch = {
    countryId: '',
    name: undefined,
    startDate: undefined
};

const validation = {
    error: "",
    competition: "",
    team: ""
}

const CompetitionTeamsCreateView = (props: IFormTwoEntityProps<ICompetitionTeam, ICompetitionSearch>) => {

    const [competitions, setCompetitions] = useState([] as ICompetition[]);
    const [teams, setTeams] = useState([] as ITeam[]);
    const [countries, setCountries] = useState([] as ICountry[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [added, setSubmitStatus] = useState({ submitStatus: false });
    const [alertMessage, setAlertMessage] = useState(validation);
    const [registered, setRegisterStatus] = useState({ status: "" });
    const appState = useContext(AppContext);

    const loadData = async (searchCompetition?: ICompetitionSearch) => {

        let competitionsUri = '/Competitions';
        if (searchCompetition) {
            competitionsUri += "/search" +
                "?countryId=" + searchCompetition.countryId
        }

        let competitions = await BaseService.getAll<ICompetition>(competitionsUri, appState.token);
        let teams = await BaseService.getAll<ITeam>('/Teams/myTeams', appState.token);
        let countries = await BaseService.getAll<ICountry>('/Countries');

        if (competitions.ok && competitions.data && teams.ok && teams.data && countries.ok && countries.data) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            setCompetitions(competitions.data);
            setCountries(countries.data);
            setTeams(teams.data);
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: -1 });
        }
    }

    const handleValidation = () => {
        let formIsValid = true;
        setAlertMessage(validation);

        if (props.firstValues.teamId === "-1" || !props.firstValues.teamId) {
            setAlertMessage(prevState => ({
                ...prevState,
                team: "Team field is required."
            }));
            formIsValid = false;
        }

        if (props.firstValues.competitionId === "-1" || !props.firstValues.competitionId) {
            setAlertMessage(prevState => ({
                ...prevState,
                competition: "Competition field is required."
            }));
            formIsValid = false;
        }

        return formIsValid;
    }

    const getSearchCompetition = (e: Event) => {
        e.preventDefault();
        loadData(props.secondValues as ICompetitionSearch);
    }

    const registerClicked = async (e: Event) => {
        e.preventDefault();

        if (!handleValidation()) {
            return;
        };

        props.firstValues.since = new Date();
        let response = await BaseService.post<ICompetitionTeam>('/CompetitionTeams', props.firstValues, appState.token!);
        if (response.ok) {
            setSubmitStatus({ submitStatus: true });
        } else if (response.statusCode === 400) {
            setRegisterStatus({ status: "The team is already registered to the competition!" })
        } else {
            setAlertMessage(prevState => ({ ...prevState, error: "Error code: " + response.statusCode.toString() }));
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps


    if (competitions && teams) {
        return (
            <>
                { appState.token === null ? <Redirect to="/identity/login" /> : null}
                { added.submitStatus === true ? <Redirect to="/" /> : null}

                <h4 className="text-center">Register to competition</h4>
                { registered.status !== "" ? <><p className="errorClass">The team is already registered to the competition!</p></> : null}
                <Alert show={alertMessage.error !== ''} message={alertMessage.error} alertClass={EAlertClass.Danger} />
                <hr />
                <div className="col-md-4">
                <form method="get">
                        <div className="form-group">
                            <div className="row">
                                <div className="col">
                                <label className="text-center" htmlFor="Search_CountryId">Sort competitions by country</label>
                                    <select value={props.secondValues.countryId} onChange={(e) => props.handleChange(e.target)} name="countryId" className="form-control" id="Search_CountryId">
                                        <option></option>
                                        {countries.map(country => {
                                            return <option key={country.id} value={country!.id!}>{country.name}</option>
                                        })}
                                    </select>
                                </div>
                            </div>
                            <br />
                            <div className="text-center">
                                <div className="form-group">
                                    <input type="submit" value="Search" onClick={(e) => getSearchCompetition(e.nativeEvent)} className="btn btn-primary" />
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
                <hr />
                <p className="redStar">* Field is required</p>
                <div className="row">
                    <div className="col-md-4">
                        <form>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="Competition_TeamId">Team name</label> *
                                </div>
                                <select value={props.firstValues.teamId} onChange={(e) => props.handleChange(e.target)} className="form-control" data-val="true" id="Competition_TeamId" name="Competition.TeamId">
                                    <option value="-1">---Please select---</option>
                                    {teams.map(team => {
                                        return <option key={team.id} value={team!.id!}>{team.name}</option>
                                    })}
                                </select>
                                {alertMessage.team !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.team}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <div>
                                    <label className="control-label" htmlFor="Competition_CompetitionId">Competition name</label> *
                            </div>
                                <select value={props.firstValues.competitionId} onChange={(e) => props.handleChange(e.target)} className="form-control" data-val="true" id="Competition_CompetitionId" name="Competition.CompetitionId">
                                    <option value="-1">---Please select---</option>
                                    {competitions.map(competition => {
                                        return <option key={competition.id} value={competition!.id!}>{competition.name}</option>
                                    })}
                                </select>
                                {alertMessage.competition !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.competition}</span> </> : null}
                            </div>
                            <div className="form-group">
                                <input type="submit" onClick={(e) => registerClicked(e.nativeEvent)} value="Register" className="btn btn-primary" />
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

const CompetitionTeamsCreate = () => {

    const [formValues, setFormValues] = useState(initialFormValues);
    const [searchValues, setSearchValues] = useState(initialSearchValues);
    const { id } = useParams() as IRouteId;

    useEffect(() => {
        if (id != null) {
            setFormValues({ ...formValues, competitionId: id });
        }
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const handleChange = (target: HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement) => {

        if (target.id === 'Competition_TeamId') {
            setFormValues({ ...formValues, teamId: target.value });
            return;
        }
        if (target.id === 'Competition_CompetitionId') {
            setFormValues({ ...formValues, competitionId: target.value });
            return;
        }
        if (target.id === 'Search_CountryId') {
            setSearchValues({ ...searchValues, countryId: target.value });
            return;
        }
    }
    return <CompetitionTeamsCreateView firstValues={formValues} secondValues={searchValues} handleChange={handleChange} />
};

export default CompetitionTeamsCreate;