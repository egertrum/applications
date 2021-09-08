import { useContext, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Loader from "../../components/Loader";
import { AppContext } from "../../context/AppContext";
import { BaseService } from "../../services/base-service";
import { EPageStatus } from "../../types/EPageStatus";
import { IReport } from "../../types/IReport";

const RowDisplay = (props: { report: IReport }) => {
    return (
        <>
            { props.report.submitter && props.report.appUserId ?
                <tr>
                    <td>
                        {props.report.submitter!}
                    </td>
                    <td>
                        {props.report.appUserId}
                    </td>
                    <td>
                        {props.report.title}
                    </td>
                    <td>
                        <Link className="text-dark" to={'/reports/delete/' + props.report.id}>Delete</Link>
                    </td>
                </tr> 
                : null}
        </>
    );
}

const ReportIndex = () => {

    const [reports, setReports] = useState([] as IReport[]);
    const [pageStatus, setPageStatus] = useState({ pageStatus: EPageStatus.Loading, statusCode: -1 });
    const appState = useContext(AppContext);
    
    const loadData = async () => {
        let result = await BaseService.getAll<IReport>('/Reports', appState.token!);

        if (result.ok && result.data) {
            setPageStatus({ pageStatus: EPageStatus.OK, statusCode: 0 });
            setReports(result.data);
        } else {
            setPageStatus({ pageStatus: EPageStatus.Error, statusCode: result.statusCode });
        }
    }

    useEffect(() => {
        loadData();
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    return (

        <>
            <h4 className="text-center">Reports</h4>
            <table className="table table-hover">
                <thead>
                    <tr>
                        <th>
                            Submitter
                        </th>
                        <th>
                            App User
                        </th>
                        <th>
                            Title
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {reports.map(report =>
                        <RowDisplay report={report} key={report.id} />)
                    }
                </tbody>
            </table>
            <Loader {...pageStatus} />
        </>
    );
}

export default ReportIndex;