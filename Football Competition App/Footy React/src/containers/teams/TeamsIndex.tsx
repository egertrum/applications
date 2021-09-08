import { useContext, useEffect, useState } from "react";
import { Link, Redirect } from "react-router-dom";
import Loader from "../../components/Loader";
import { AppContext } from "../../context/AppContext";
import { ITeam } from "../../dto/ITeam";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import pin from '../../wwwroot/images/pin.png';
import arrow from '../../wwwroot/images/right-arrow.png';

const RowDisplay = (props: { team: ITeam, myTeams: boolean }) => {

    const appState = useContext(AppContext);

    return (
        <tr>
            <td>
                <img className="very-small-icon" src={pin} alt="" />
                {props.team.country!.name}
            </td>
            <td>
                {props.team.name}
            </td>
            <td>
                {props.team.playersAmount}
            </td>
            <td>
                <Link className="text-dark" to={'/teams/' + props.team.id}>More Info</Link>
                {appState.role === "Admin" || props.myTeams === true ?
                    <>
                        |
                    <Link className="text-dark" to={'/teams/edit/' + props.team.id}>Edit</Link>
                    |
                    <Link className="text-dark" to={'/teamPersons/create/id=' + props.team.id}>Add member</Link>
                    </> : null}
                {appState.role === "Admin"?
                    <>
                    |
                    <Link className="text-dark" to={'/teams/delete/' + props.team.id}>Delete</Link>
                    </> : null}
            </td>
        </tr>
    );
}

const TeamsIndex = () => {

    const [teams, setTeams] = useState([] as ITeam[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [myTeams] = useState({ myTeamsSet: false });
    const appState = useContext(AppContext);

    const loadData = async () => {
        let teamUri = '/Teams';

        if (myTeams.myTeamsSet === true) {
            teamUri += "/myTeams";
        }

        let result = await BaseService.getAll<ITeam>(teamUri, appState.token);

        if (result.ok && result.data) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            setTeams(result.data);
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
        }
    }

    useEffect(() => {
        if (appState.role !== "Admin") {
            myTeams.myTeamsSet = true;
        }
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    return (
        <>
            { appState.token === null ? <Redirect to="/identity/login" /> : null}

            {myTeams.myTeamsSet === true ?
                <>
                    <h4 className="text-center">My Teams</h4>
                </>
                :
                <>
                    <h4 className="text-center">Teams</h4>
                </>
            }

            {myTeams.myTeamsSet === true && appState.token !== null ?
                <>
                    <p className="text-center">
                        <Link className="text-dark" to="/teams/create">Create new team
                        <img className="extra-small-icon" src={arrow} alt="" /></Link>
                    </p>
                    <p className="text-center">
                        <Link className="text-dark" to="/persons/create" >Add new members
                        <img className="extra-small-icon" src={arrow} alt="" /></Link>
                    </p>
                </> : null}

            <table className="table table-hover">
                <thead>
                    <tr>
                        <th>
                        </th>
                        <th>
                            Team name
                        </th>
                        <th>
                            Team members
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {teams.map(team =>
                        <RowDisplay team={team} myTeams={myTeams.myTeamsSet} key={team.id} />)
                    }
                </tbody>
            </table>
            <Loader {...pageStatus} />
        </>
    );
}

export default TeamsIndex;