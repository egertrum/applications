import { useContext, useState } from 'react';
import { Redirect } from 'react-router-dom';
import Alert, { EAlertClass } from '../../components/Alert';
import { AppContext } from '../../context/AppContext';
import { BaseService } from '../../services/base-service';
import { IFormProps } from '../../types/IFormProps';
import { IReport } from '../../types/IReport';

const initialFormValues: IReport = {
    title: '',
    comment: ''
};

const validation = {
    error: "",
    title: "",
    comment: ""
}

const ReportCreateView = (props: IFormProps<IReport>) => {

    const appState = useContext(AppContext);
    const [added, setSubmitStatus] = useState({ submitStatus: false });
    const [alertMessage, setAlertMessage] = useState(validation);

    const handleValidation = () => {
        let formIsValid = true;
        setAlertMessage(validation);

        if(!props.values.title) {
            setAlertMessage(prevState => ({
                ...prevState,
                title: "Title field is required."
            }));
            formIsValid = false;
        }

        if(!props.values.comment) {
            setAlertMessage(prevState => ({
                ...prevState,
                comment: "Comment field is required."
            }));
            formIsValid = false;
        }
        
       return formIsValid;
   }

    const submitClicked = async (e: Event) => {
        e.preventDefault();

        if (!handleValidation()) {
            return;
        };

        let response = await BaseService.post<IReport>('/Reports', props.values, appState.token!);
        if (response.ok) {
            setSubmitStatus({ submitStatus: true });
        } else {
            setAlertMessage(prevState => ({...prevState, error: "Error code: " + response.statusCode.toString()}));
        }
    }

    return (
        <>
            { added.submitStatus === true ? <Redirect to="/" /> : null}

            <h4 className="text-center">Report a Problem</h4>
            <Alert show={alertMessage.error !== ''} message={alertMessage.error} alertClass={EAlertClass.Danger} />
            <hr/>
            <div className="row">
                <div className="col-md-4">
                    <form>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Report_Title">Title</label>
                            <input value={props.values.title} onChange={(e) => props.handleChange(e.target)} className="form-control" type="text" id="Report_Title"/>
                            {alertMessage.title !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.title}</span> </> : null}
                        </div>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Report_Comment">Name</label>
                            <textarea value={props.values.comment} onChange={(e) => props.handleChange(e.target)} className="form-control" id="Report_Comment"></textarea>
                            {alertMessage.comment !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.comment}</span> </> : null}
                        </div>
                        <div className="form-group">
                            <input type="submit" onClick={(e) => submitClicked(e.nativeEvent)} value="Create" className="btn btn-primary" />
                        </div>
                    </form>
                </div>
            </div>
        </>
    );
}

const ReportCreate = () => {

    const [formValues, setFormValues] = useState(initialFormValues);

    const handleChange = (target: HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement) => {
        if (target.id === 'Report_Title') {
            setFormValues({ ...formValues, title: target.value });
            return;
        }
        if (target.id === 'Report_Comment') {
            setFormValues({ ...formValues, comment: target.value });
            return;
        }
    }
    return <ReportCreateView values={formValues} handleChange={handleChange} />
};

export default ReportCreate;