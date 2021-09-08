import { Link, useParams } from "react-router-dom";
import { IRouteId } from "../../types/IRouteId";
import { useContext, useEffect, useState } from "react";
import Loader from "../../components/Loader";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import arrow from '../../wwwroot/images/right-arrow.png';
import { ITeam } from "../../dto/ITeam";
import { AppContext } from "../../context/AppContext";
import { ITeamPerson } from "../../dto/ITeamPerson";

const RowDisplay = (props: { teamPerson: ITeamPerson, belongsToUser: boolean }) => {
    var dateformat = require("dateformat");

    return (
        <tr>
            <td>
                {props.teamPerson.person!.firstName} {props.teamPerson.person!.lastName}
            </td>
            <td>
                {props.teamPerson.role!.name!}
            </td>
            <td>
                {dateformat(props.teamPerson.since, "dd/mm/yyyy")}
            </td>
            <td>
                <Link className="text-dark" to={'/persons/' + props.teamPerson.personId}>Person details</Link>
                {props.belongsToUser ? 
                <>
                    |
                    <Link className="text-dark" to={'/teamPersons/delete/' + props.teamPerson.id}>Remove from team</Link>
                </>
                : null}
            </td>
        </tr>
    );
}

const TeamsDetails = () => {

    const { id } = useParams() as IRouteId;
    const [team, setTeam] = useState({} as ITeam);
    const [belongsToUser, setUserTeam] = useState({ belongs: false });
    const [teamMembers, setTeamMembers] = useState([] as ITeamPerson[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const appState = useContext(AppContext);

    useEffect(() => {
        const loadData = async () => {
            let teamResult = await BaseService.get<ITeam>('/Teams/' + id);
            let teamPersonsResult = await BaseService.getAll<ITeamPerson>('/TeamPersons/team?teamId=' + id);

            if (appState.token !== null) {
                let belongsResult = await BaseService.get<boolean>('/Teams/belongsToUser?id=' + id, appState.token!);
                if (belongsResult.ok && belongsResult.data === true) {
                    setUserTeam({ belongs: true });
                }
            }

            if (teamResult.ok && teamResult.data && teamPersonsResult.ok && teamPersonsResult.data) {
                setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
                setTeam(teamResult.data);
                setTeamMembers(teamPersonsResult.data)
            } else {
                setPageStatus({ pageStatus: EPageStatus.Error, statusCode: teamResult.statusCode });
            }
        }
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    if (team && team.country && teamMembers) {
        return (
            <>
                <h4 className="text-center">{team.name}</h4>

                <div>

                    <hr />
                    <dl className="row">
                        <dt className="col-sm-4">
                            Country
                        </dt>
                        <dd className="col-sm-8">
                            {team.country.name}
                        </dd>
                        <dt className="col-sm-4">
                            Team name
                        </dt>
                        <dd className="col-sm-8">
                            {team.name}
                        </dd>
                        <dt className="col-sm-4">
                            Amount of players
                        </dt>
                        <dd className="col-sm-8">
                            {team.playersAmount}
                        </dd>
                    </dl>
                </div>

                {belongsToUser.belongs ?
                    <>
                        <h6>
                            <Link className="text-dark" to={'/teams/edit/' + team.id}>Edit</Link>
                        </h6>
                    </> : null}

                <hr />
                <h4 className="text-center">Team Members</h4>

                {belongsToUser.belongs ?
                    <>
                        <p className="text-center">
                            <Link className="text-dark" to={'/teamPersons/create/id=' + team.id}>Add new team member to team
                                <img className="extra-small-icon" src={arrow} alt="" />
                            </Link>
                        </p>
                    </> : null}

                <div className="row-cols-1">
                    <div className="text-center">
                        <table className="table">
                            <thead>
                                <tr>
                                    <th>
                                        Team member
                                    </th>
                                    <th>
                                        Role
                                    </th>
                                    <th>
                                        Team member since
                                    </th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                {teamMembers.map(teamPerson =>
                                    <RowDisplay teamPerson={teamPerson} belongsToUser={belongsToUser.belongs} key={teamPerson.id} />)
                                }
                            </tbody>
                        </table>
                    </div>
                </div>
            </>
        );
    }
    return (
        <Loader {...pageStatus} />
    );
}

export default TeamsDetails;