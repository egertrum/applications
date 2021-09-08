import { useContext, useEffect, useState } from "react";
import { Link, Redirect, useParams } from "react-router-dom";
import Loader from "../../components/Loader";
import { AppContext } from "../../context/AppContext";
import { ICompetitionTeam } from "../../dto/ICompetitionTeam";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { IRouteId } from "../../types/IRouteId";

const RowDisplay = (props: { competitionTeam: ICompetitionTeam }) => {

    var dateformat = require("dateformat");

    return (
        <tr>
            <td>
                {props.competitionTeam.team!.name}
            </td>
            <td>
                {props.competitionTeam.competition!.name}
            </td>
            <td>
                {dateformat(props.competitionTeam.since, "dd/mm/yyyy")}
            </td>
            <td>
                <Link className="text-dark" to={'/competitionTeams/delete/' + props.competitionTeam.id}>Remove</Link>
            </td>
        </tr>
    );
}

const CompetitionTeamIndex = () => {
    const [competitionTeams, setCompetitionTeams] = useState([] as ICompetitionTeam[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [title, setTitle] = useState({ name: "Competitions teams"} );
    const [currentId, setCurrentId] = useState({ current: "" });
    const appState = useContext(AppContext);
    const { id } = useParams() as IRouteId;

    const loadData = async () => {
        let compTeamsUri = "/CompetitionTeams";

        if (id === "team") {
            compTeamsUri += "/teamManager"
            setTitle({ name: "My teams at competitions" });
            setCurrentId({ current: id });
        } else if (id === "organiser") {
            compTeamsUri +="/organiser";
            setTitle({ name: "Teams attending my competitions" });
            setCurrentId({ current: id });
        } else if (appState.role !== "Admin") {
            return;
        }

        let result = await BaseService.getAll<ICompetitionTeam>(compTeamsUri, appState.token);

        if (result.ok && result.data) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            setCompetitionTeams(result.data);
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
        }
    }

    if (id !== currentId.current && id !== "" && id !== undefined) {
        loadData();
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    return (

        <>
            { appState.token === null ? <Redirect to="/identity/login" /> : null}

            <h4 className="text-center">{title.name}</h4>

            <table className="table table-hover">
                <thead>
                    <tr>
                        <th>
                            Team name
                        </th>
                        <th>
                            Competition name
                        </th>
                        <th>
                            Date registered
                        </th>
                        <th>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {competitionTeams.map(compTeam =>
                        <RowDisplay competitionTeam={compTeam} key={compTeam.id} />)
                    }
                </tbody>
            </table>
            <Loader {...pageStatus} />
        </>
    );
}

export default CompetitionTeamIndex;