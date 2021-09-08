import { useParams } from "react-router-dom";
import { IRouteId } from "../../types/IRouteId";
import { useContext, useEffect, useState } from "react";
import Loader from "../../components/Loader";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { EDelete } from "../../types/EDelete";
import { Redirect } from "react-router";
import { AppContext } from "../../context/AppContext";
import { ICompetitionTeam } from "../../dto/ICompetitionTeam";


const CompetitionTeamDelete = () => {
    const { id } = useParams() as IRouteId;
    const appState = useContext(AppContext);
    const [competitionTeam, setCompetitionTeam] = useState({} as ICompetitionTeam);
    const [canDelete] = useState({ allowed: true });
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [deleted, setDelete] = useState({ deleteStatus: EDelete.NotDeleted });
    var dateformat = require("dateformat");

    useEffect(() => {
        const loadData = async () => {
            let allowed = await BaseService.get<boolean>('/CompetitionTeams/allowed?id=' + id, appState.token!);
            if (!allowed.ok || (allowed.ok && allowed.data === false)) {
                canDelete.allowed = false;
                return;
            }
    
            let result = await BaseService.get<ICompetitionTeam>('/CompetitionTeams/' + id, appState.token!);
    
            if (result.ok && result.data) {
                setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
                setCompetitionTeam(result.data);
            } else {
                setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
            }
        }
        
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const deleteClicked = async (e: Event) => {
        e.preventDefault();
        let response = await BaseService.delete<ICompetitionTeam>('/CompetitionTeams/' + id, appState.token!);
        if (response.ok) {
            setDelete({deleteStatus: EDelete.Deleted});
        }
    }

    if (appState.token === null) {
        return <>
            <Redirect to={"/"}/>
        </>
    }

    if (competitionTeam && competitionTeam.team && competitionTeam.competition) {
        return (
            <>
            { canDelete.allowed !== true ? <Redirect to="/" /> : null}
            { deleted.deleteStatus === EDelete.Deleted ? <Redirect to="/teams" /> : null}
                <form>
                    <h4 className="text-center">Are you sure you want to remove this team from this competition?</h4>
                    <div>
                        <hr />
                        <dl className="row">
                            <dt className="col-sm-4">
                                Team
                            </dt>
                            <dd className="col-sm-8">
                                {competitionTeam.team.name}
                            </dd>
                            <dt className="col-sm-4">
                                Competition
                            </dt>
                            <dd className="col-sm-8">
                                {competitionTeam.competition.name}
                            </dd>
                            <dt className="col-sm-4">
                                Registered at
                            </dt>
                            <dd className="col-sm-8">
                                {dateformat(competitionTeam.since, "dd/mm/yyyy")}
                            </dd>
                        </dl>
                        <div className="form-group">
                            <button onClick={(e) => deleteClicked(e.nativeEvent)} type="submit" className="btn btn-danger">Remove</button>
                        </div>
                    </div>
                </form>
            </>
        );
    }
    return (
        <>
            <Loader {...pageStatus} />
        </>
    );
}

export default CompetitionTeamDelete;