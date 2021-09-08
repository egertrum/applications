import { useContext, useEffect, useState } from 'react';
import { Redirect, useParams } from 'react-router-dom';
import Alert, { EAlertClass } from '../../components/Alert';
import Loader from '../../components/Loader';
import { AppContext } from '../../context/AppContext';
import { IPerson } from '../../dto/IPerson';
import { IRole } from '../../dto/IRole';
import { ITeam } from '../../dto/ITeam';
import { ITeamPerson } from '../../dto/ITeamPerson';
import { BaseService } from '../../services/base-service';
import { EPageStatus } from '../../types/EPageStatus';
import { IFormProps } from '../../types/IFormProps';

const initialFormValues: ITeamPerson = {
    teamId: '',
    personId: '',
    roleId: '',
    since: null,
    until: null
};

const validation = {
    error: "",
    team: "",
    role: "",
    person: "",
    since: ""
}

const TeamPersonCreateView = (props: IFormProps<ITeamPerson>) => {

    const [teams, setTeams] = useState([] as ITeam[]);
    const [persons, setPersons] = useState([] as IPerson[]);
    const [roles, setRoles] = useState([] as IRole[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [alertMessage, setAlertMessage] = useState(validation);
    const [added, setSubmitStatus] = useState({ submitStatus: false });
    const appState = useContext(AppContext);
    var dateformat = require("dateformat");

    const loadData = async () => {
        let teamsResult = await BaseService.getAll<ITeam>('/Teams/myTeams', appState.token);
        let personsResult = await BaseService.getAll<IPerson>('/Persons', appState.token);
        let rolesResult = await BaseService.getAll<IRole>('/PersonRoles', appState.token);

        if (teamsResult.ok && teamsResult.data && personsResult.ok && personsResult.data && rolesResult.ok && rolesResult.data) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            setPersons(personsResult.data);
            setTeams(teamsResult.data);
            setRoles(rolesResult.data);
        }
    }

    const handleValidation = () => {
        let formIsValid = true;
        setAlertMessage(validation);

        if(props.values.teamId === "-1" || !props.values.teamId) {
            setAlertMessage(prevState => ({
                ...prevState,
                team: "Team field is required."
            }));
            formIsValid = false;
        }

        if(props.values.roleId === "-1" || !props.values.roleId) {
            setAlertMessage(prevState => ({
                ...prevState,
                role: "Role field is required."
            }));
            formIsValid = false;
        }

        if(props.values.personId === "-1" || !props.values.personId) {
            setAlertMessage(prevState => ({
                ...prevState,
                person: "Person field is required."
            }));
            formIsValid = false;
        }

        if(!props.values.since) {
            setAlertMessage(prevState => ({
                ...prevState,
                since: "Since field is required."
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

        let response = await BaseService.post<ITeamPerson>('/TeamPersons', props.values, appState.token!);
        if (response.ok) {
            setSubmitStatus({ submitStatus: true });
        } else if (response.statusCode === 400) {
            setAlertMessage(prevState => ({...prevState, error: "Person is already added to the team."}));
        } else if(response.statusCode === 404) {
            setAlertMessage(prevState => ({...prevState, error: "Not allowed!"}));
        } else {
            setAlertMessage(prevState => ({...prevState, error: response.statusCode.toString()}));
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    return (
        <>
            <Loader {...pageStatus} />
            { appState.token === null ? <Redirect to="/identity/login" /> : null}
            { added.submitStatus === true ? <Redirect to="/teams" /> : null}

            <h4 className="text-center">Add new team member to team</h4>
            <Alert show={alertMessage.error !== ''} message={alertMessage.error} alertClass={EAlertClass.Danger} />
            <hr />
            <div className="row">
                <div className="col-md-4">
                <p className="redStar">* Field is required</p>
                    <form action="/TeamPersons/Create" method="post">
                        <div className="form-group">
                            <div>
                                <label className="control-label" htmlFor="TeamPerson_TeamId">Team</label> *
                            </div>
                            <select value={props.values.teamId} onChange={(e) => props.handleChange(e.target)} className="form-control" data-val="true" id="TeamPerson_TeamId" name="TeamPerson.TeamId">
                                <option value="-1">---Please select---</option>
                                {teams.map(team => {
                                    if (team.id === props.values.teamId) {
                                        return <option key={team.id} selected value={team!.id!}>{team.name}</option> 
                                    }
                                    return <option key={team.id} value={team!.id!}>{team.name}</option> 
                                })}
                            </select>
                            {alertMessage.team !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.team}</span> </> : null}
                        </div>
                        <div className="form-group">
                            <div>
                                <label className="control-label" htmlFor="TeamPerson_PersonId">Team member</label> *
                            </div>
                            <select value={props.values.personId} onChange={(e) => props.handleChange(e.target)} className="form-control" data-val="true" id="TeamPerson_PersonId" name="TeamPerson.PersonId">
                                <option value="-1">---Please select---</option>
                                {persons.map(person => {
                                    return <option key={person.id} value={person!.id!}>{person.firstName} {person.lastName}</option> 
                                })}
                            </select>
                            {alertMessage.person !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.person}</span> </> : null}
                        </div>
                        <div className="form-group">
                            <div>
                                <label className="control-label" htmlFor="TeamPerson_RoleId">Team member role</label> *
                            </div>
                            <select value={props.values.roleId} onChange={(e) => props.handleChange(e.target)} className="form-control" data-val="true" id="TeamPerson_RoleId" name="TeamPerson.RoleId">
                                <option value="-1">---Please select---</option>
                                {roles.map(role => {
                                    return <option key={role.id} value={role!.id!}>{role.name}</option> 
                                })}
                            </select>
                            {alertMessage.role !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.role}</span> </> : null}
                        </div>
                        <div className="form-group">
                            <div>
                                <label className="control-label" htmlFor="TeamPerson_Since">Since</label> *
                            </div>
                            <input className="form-control" value={props.values.since !== null ? dateformat(props.values.since, "isoDate") : ''} onChange={(e) => props.handleChange(e.target)} type="date" id="TeamPerson_Since" name="TeamPerson.Since"/>
                            {alertMessage.since !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.since}</span> </> : null}
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="TeamPerson_Until">Until</label>
                            <input className="form-control" value={props.values.until !== null ? dateformat(props.values.until, "isoDate") : ''} onChange={(e) => props.handleChange(e.target)} type="date" id="TeamPerson_Until" name="TeamPerson.Until"/>
                        </div>
                        <div className="form-group">
                            <input type="submit" onClick={(e) => submitClicked(e.nativeEvent)} value="Add" className="btn btn-primary" />
                        </div>
                    </form>
                </div>
            </div>
        </>
    );
}

const TeamPersonCreate = () => {

    const [formValues, setFormValues] = useState(initialFormValues);
    const { person, id } = useParams() as { person: string, id: string };

    useEffect(() => {
        if (id) {
            setFormValues({ ...formValues, teamId: id });
        } if (person) {
            setFormValues({ ...formValues, personId: person });
        }
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const handleChange = (target: HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement) => {

        if (target.id === 'TeamPerson_TeamId') {
            setFormValues({ ...formValues, teamId: target.value });
            return;
        }
        if (target.id === 'TeamPerson_PersonId') {
            setFormValues({ ...formValues, personId: target.value });
            return;
        }
        if (target.id === 'TeamPerson_RoleId') {
            setFormValues({ ...formValues, roleId: target.value });
            return;
        }
        if (target.id === 'TeamPerson_Since') {
            setFormValues({ ...formValues, since: target.value as unknown as Date });
            return;
        }
        if (target.id === 'TeamPerson_Until') {
            setFormValues({ ...formValues, until: target.value as unknown as Date });
            return;
        }
    }
    return <TeamPersonCreateView values={formValues} handleChange={handleChange} />
};

export default TeamPersonCreate;