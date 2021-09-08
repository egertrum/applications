import { useParams } from "react-router-dom";
import { IRouteId } from "../../types/IRouteId";
import { useEffect, useState, useContext } from "react";
import Loader from "../../components/Loader";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { Redirect } from "react-router";
import { AppContext } from "../../context/AppContext";
import { EDelete } from "../../types/EDelete";
import { ITeamPerson } from "../../dto/ITeamPerson";


const TeamPersonDelete = () => {

    const { id } = useParams() as IRouteId;
    const [teamPerson, setTeamPerson] = useState({} as ITeamPerson);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [deleted, setDelete] = useState({ deleteStatus: EDelete.NotDeleted });
    const appState = useContext(AppContext);
    var dateformat = require("dateformat");

    useEffect(() => {
        const loadData = async () => {
            let personResult = await BaseService.get<ITeamPerson>('/TeamPersons/' + id, appState.token!);

            if (personResult.ok && personResult.data) {
                setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
                setTeamPerson(personResult.data);
            } else {
                setPageStatus({ pageStatus: EPageStatus.Error, statusCode: personResult.statusCode });
            }
        }
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const deleteClicked = async (e: Event) => {
        e.preventDefault();
        let response = await BaseService.delete<ITeamPerson>('/TeamPersons/' + id, appState.token!);
        if (response.ok) {
            setDelete({deleteStatus: EDelete.Deleted});
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: response.statusCode });
        }
    }

    if (teamPerson && teamPerson.person && teamPerson.team && teamPerson.role) {
        return (
            <>
            { deleted.deleteStatus === EDelete.Deleted ? <Redirect to="/teams" /> : null}
                <form>
                    <h4 className="text-center">Remove team member</h4>
                    <h5 className="text-center">Are you sure you want to remove this member from your team?</h5>
                    
                    <div>
                        <hr />
                        <dl className="row">
                            <dt className="col-sm-4">
                                Team
                            </dt>
                            <dd className="col-sm-6">
                                {teamPerson.team.name}
                            </dd>
                            <dt className="col-sm-4">
                                Member
                            </dt>
                            <dd className="col-sm-6">
                                {teamPerson.person.firstName} {teamPerson.person.lastName}
                            </dd>
                            <dt className="col-sm-4">
                                Role
                            </dt>
                            <dd className="col-sm-6">
                                {teamPerson.role?.name}
                            </dd>
                            <dt className="col-sm-4">
                                Since
                            </dt>
                            <dd className="col-sm-6">
                                {dateformat(teamPerson.since, "dd/mm/yyyy")}
                            </dd>
                            <dt className="col-sm-4">
                                Until
                            </dt>
                            <dd className="col-sm-6">
                                {teamPerson.until ? dateformat(teamPerson.until, "dd/mm/yyyy") : ''}
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
        <Loader {...pageStatus} />
    );
}

export default TeamPersonDelete;