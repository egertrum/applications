import { Redirect, useParams } from "react-router-dom";
import { IRouteId } from "../../types/IRouteId";
import { useEffect, useState } from "react";
import Loader from "../../components/Loader";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { IPerson } from "../../dto/IPerson";


const PersonDetails = () => {
    const { id } = useParams() as IRouteId;
    const [person, setPerson] = useState({} as IPerson);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    var dateformat = require("dateformat");

    useEffect(() => {
        const loadData = async () => {
            let personResult = await BaseService.get<IPerson>('/Persons/' + id);

            if (personResult.ok && personResult.data) {
                setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
                setPerson(personResult.data);
            } else {
                setPageStatus({ pageStatus: EPageStatus.Error, statusCode: personResult.statusCode });
            }
        }
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    if (pageStatus.statusCode === 500) {
        return <><Redirect to={"/"}/></>
    }

    if (person && person.country) {
        return (
            <>
                <h4 className="text-center">{person.firstName} {person.lastName}</h4>

                <div>
                    <hr />
                    <dl className="row">
                        <dt className="col-sm-4">
                            Birth country
                        </dt>
                        <dd className="col-sm-6">
                            {person.country.name}
                        </dd>
                        <dt className="col-sm-4">
                            First name
                        </dt>
                        <dd className="col-sm-6">
                            {person.firstName}
                        </dd>
                        <dt className="col-sm-4">
                            Last name
                        </dt>
                        <dd className="col-sm-6">
                            {person.lastName}
                        </dd>
                        <dt className="col-sm-4">
                            Teams associated with
                        </dt>
                        <dd className="col-sm-6">
                            {Object.entries(person.personTeams!).map(([team, games], index) => {
                                if (index !== 0) {
                                    return <span key={index}> | {team}</span>
                                } 
                                return <span key={index}>{team}</span>
                            })}
                        </dd>
                        <dt className="col-sm-4">
                            Date of birth
                        </dt>
                        <dd className="col-sm-6">
                            {dateformat(person.birthDate, "dd/mm/yyyy")}
                        </dd>
                        <dt className="col-sm-4">
                            Gender
                        </dt>
                        <dd className="col-sm-6">
                            {person.gender}
                        </dd>
                    </dl>
                </div>
            </>
        );
    }
    return (
        <Loader {...pageStatus} />
    );
}

export default PersonDetails;