import { useContext, useState } from 'react';
import { Redirect } from 'react-router-dom';
import Alert, { EAlertClass } from '../../components/Alert';
import { AppContext } from '../../context/AppContext';
import { ICountry } from '../../dto/ICountry';
import { BaseService } from '../../services/base-service';
import { IFormProps } from '../../types/IFormProps';

const initialFormValues: ICountry = {
    name: ''
};

const validation = {
    error: "",
    name: ""
}

const CountryCreateView = (props: IFormProps<ICountry>) => {

    const appState = useContext(AppContext);
    const [alertMessage, setAlertMessage] = useState(validation);
    const [added, setSubmitStatus] = useState({ submitStatus: false });

    const handleValidation = () => {
        let formIsValid = true;
        setAlertMessage(validation);

        if(!props.values.name) {
            setAlertMessage(prevState => ({
                ...prevState,
                name: "Name field is required."
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

        let response = await BaseService.post<ICountry>('/Countries', props.values, appState.token!);
        if (response.ok) {
            setSubmitStatus({ submitStatus: true });
        } else {
            setAlertMessage(prevState => ({...prevState, error: "Error code: " + response.statusCode.toString()}));
        } 
    }

    return (
        <>
            { appState.role !== "Admin" ? <Redirect to="/identity/login" /> : null}
            { added.submitStatus === true ? <Redirect to="/countries" /> : null}
            <h1>Add</h1>
            <h4>Competition</h4>
            <Alert show={alertMessage.error !== ''} message={alertMessage.error} alertClass={EAlertClass.Danger} />
            <hr/>
            <div className="row">
                <div className="col-md-4">
                    <form>
                        <div className="form-group">
                            <label className="control-label" htmlFor="Competition_Name">Name</label>
                            <input value={props.values.name} onChange={(e) => props.handleChange(e.target)} className="form-control" type="text" id="Country_Name"/>
                            {alertMessage.name !== '' ? <> <span className="text-danger field-validation-valid">{alertMessage.name}</span> </> : null}
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

const CountryCreate = () => {

    const [formValues, setFormValues] = useState(initialFormValues);

    const handleChange = (target: HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement) => {
        if (target.id === 'Country_Name') {
            setFormValues({ ...formValues, name: target.value });
            return;
        }
    }
    return <CountryCreateView values={formValues} handleChange={handleChange} />
};

export default CountryCreate;