import { useParams } from "react-router-dom";
import { IRouteId } from "../../types/IRouteId";
import { useContext, useEffect, useState } from "react";
import Loader from "../../components/Loader";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { EDelete } from "../../types/EDelete";
import { Redirect } from "react-router";
import { ICountry } from "../../dto/ICountry";
import { AppContext } from "../../context/AppContext";


const CountryDelete = () => {
    
    const { id } = useParams() as IRouteId;

    const appState = useContext(AppContext);
    const [country, setCountry] = useState({} as ICountry);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const [deleted, setDelete] = useState({ deleteStatus: EDelete.NotDeleted });

    const loadData = async () => {
        let result = await BaseService.get<ICountry>('/Countries/' + id);

        if (result.ok && result.data) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            setCountry(result.data);
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const deleteClicked = async (e: Event) => {
        e.preventDefault();
        let response = await BaseService.delete<ICountry>('/Countries/' + id, appState.token!);
        if (response.ok) {
            setDelete({deleteStatus: EDelete.Deleted});
        }
    }

    if (country) {
        return (
            <>
            { appState.role !== "Admin" ? <Redirect to="/" /> : null}
            { deleted.deleteStatus === EDelete.Deleted ? <Redirect to="/countries" /> : null}
                <form onSubmit={(e) => deleteClicked(e.nativeEvent)}>
                    <h3>Are you sure you want to delete this?</h3>

                    <div>

                        <hr />
                        <dl className="row">
                            <dt className="col-sm-2">
                                Country
                        </dt>
                            <dd className="col-sm-10">
                                {country.name}
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
        <>
            <Loader {...pageStatus} />
        </>
    );
}

export default CountryDelete;