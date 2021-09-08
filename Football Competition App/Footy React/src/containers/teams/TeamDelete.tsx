import { useParams } from "react-router-dom";
import { IRouteId } from "../../types/IRouteId";
import { useEffect, useState, useContext } from "react";
import Loader from "../../components/Loader";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { Redirect } from "react-router";
import { AppContext } from "../../context/AppContext";
import { EDelete } from "../../types/EDelete";
import { ITeam } from "../../dto/ITeam";


const TeamDelete = () => {

    const { id } = useParams() as IRouteId;
    const [team, setTeam] = useState({} as ITeam);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [deleted, setDelete] = useState({ deleteStatus: EDelete.NotDeleted });
    const appState = useContext(AppContext);

    useEffect(() => {
        const loadData = async () => {
            let result = await BaseService.get<ITeam>('/Teams/' + id, appState.token!);

            if (result.ok && result.data) {
                setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
                setTeam(result.data);
            } else {
                setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
            }
        }
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const deleteClicked = async (e: Event) => {
        e.preventDefault();
        let response = await BaseService.delete<ITeam>('/Teams/' + id, appState.token!);
        if (response.ok) {
            setDelete({deleteStatus: EDelete.Deleted});
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: response.statusCode });
        }
    }

    if (appState.role !== "Admin"){
        return <> <Redirect to={"/"}/> </>
    }

    if (team && team.country) {
        return (
            <>
            { deleted.deleteStatus === EDelete.Deleted ? <Redirect to="/teams" /> : null}
                <form onSubmit={(e) => deleteClicked(e.nativeEvent)}>
                    <h4 className="text-center">Are you sure you want to delete this?</h4>
                    <h5 className="text-center">{team.name}</h5>
                    
                    { pageStatus.statusCode === 400 ? 
                        <>
                            <div className="errorClass">Can not delete this team! Delete {team.name}'s Team persons,
                                Competition teams and Games before trying to delete this team.
                            </div>
                        </> 
                        : null
                    }
                    
                    <div>
                        <hr />
                        <dl className="row">
                            <dt className="col-sm-4">
                                Country
                            </dt>
                            <dd className="col-sm-6">
                                {team.country.name}
                            </dd>
                            <dt className="col-sm-4">
                                Team name
                            </dt>
                            <dd className="col-sm-6">
                                {team.name}
                            </dd>
                            <dt className="col-sm-4">
                                Amount of players
                            </dt>
                            <dd className="col-sm-6">
                                {team.playersAmount}
                            </dd>
                        </dl>
                        <div className="form-group">
                            <button onClick={(e) => deleteClicked(e.nativeEvent)} type="submit" className="btn btn-danger">Delete</button>
                        </div>
                    </div>
                </form>
            </>
        );
    }
    return (
        <Loader {...pageStatus} />
    );
}

export default TeamDelete;