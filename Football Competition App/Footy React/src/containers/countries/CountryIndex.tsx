import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Loader from "../../components/Loader";
import { ICountry } from "../../dto/ICountry";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";

const RowDisplay = (props: { country: ICountry }) => {
    return (
        <tr>
            <td>
                {props.country.name}
            </td>
            <td>
                <Link className="text-dark" to={'/countries/delete/' + props.country.id}>Delete</Link>
            </td>
        </tr>
    );
}

const CountryIndex = () => {
    
    const [countries, setCountries] = useState([] as ICountry[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });

    const loadData = async () => {
        let result = await BaseService.getAll<ICountry>('/Countries');

        if (result.ok && result.data) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            setCountries(result.data);
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    return (

        <>
            <h4 className="text-center">Countries</h4>
            <Link className="text-dark" to={'/countries/create'}>Create New</Link>
            <table className="table table-hover">
                <thead>
                    <tr>
                        <th>
                            Name
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {countries.map(country =>
                        <RowDisplay country={country} key={country.id} />)
                    }
                </tbody>
            </table>
            <Loader {...pageStatus} />
        </>
    );
}

export default CountryIndex;