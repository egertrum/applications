import { useParams } from "react-router-dom";
import { IRouteId } from "../../types/IRouteId";
import { useEffect, useState, useContext } from "react";
import Loader from "../../components/Loader";
import { ICompetition } from "../../dto/ICompetition";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { Redirect } from "react-router";
import { AppContext } from "../../context/AppContext";
import { EDelete } from "../../types/EDelete";


const CompetitionDelete = () => {

    const { id } = useParams() as IRouteId;
    const appState = useContext(AppContext);
    const [competition, setCompetition] = useState({} as ICompetition);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [deleted, setDelete] = useState({ deleteStatus: EDelete.NotDeleted });
    var dateformat = require("dateformat");

    const loadData = async () => {
        let result = await BaseService.get<ICompetition>('/Competitions/' + id, appState.token!);

        if (result.ok && result.data) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            setCompetition(result.data);
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const deleteClicked = async (e: Event) => {
        e.preventDefault();
        let response = await BaseService.delete<ICompetition>('/Competitions/' + id, appState.token!);
        if (response.ok) {
            setDelete({ deleteStatus: EDelete.Deleted });
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: response.statusCode });
        }
    }


    if (competition && competition.country) {
        return (
            <>
                <Loader {...pageStatus} />
                { deleted.deleteStatus === EDelete.Deleted ? <Redirect to="/" /> : null}
                <form onSubmit={(e) => deleteClicked(e.nativeEvent)}>
                    <h4 className="text-center">Are you sure you want to delete this?</h4>

                    <div>

                        <hr />
                        <dl className="row">
                            <dt className="col-sm-2">
                                Country
                            </dt>
                            <dd className="col-sm-10">
                                {competition.country.name}
                            </dd>
                            <dt className="col-sm-2">
                                Competition name
                            </dt>
                            <dd className="col-sm-10">
                                {competition.name}
                            </dd>
                            <dt className="col-sm-2">
                                Organiser
                            </dt>
                            <dd className="col-sm-10">
                                {competition.organiser}
                            </dd>
                            <dt className="col-sm-2">
                                Start date
                            </dt>
                            <dd className="col-sm-10">
                                {dateformat(competition.startDate, "dd/mm/yyyy")}
                            </dd>
                            <dt className="col-sm-2">
                                End date
                            </dt>
                            <dd className="col-sm-10">
                                {dateformat(competition.endDate, "dd/mm/yyyy")}
                            </dd>
                            <dt className="col-sm-2">
                                Comment
                            </dt>
                            <dd className="col-sm-10">
                                {competition.comment}
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

export default CompetitionDelete;