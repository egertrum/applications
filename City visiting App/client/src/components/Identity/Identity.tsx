import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { Avatar, Button, Paper, Grid, Typography, Container, TextField, Grow } from '@material-ui/core';
import { useHistory } from 'react-router-dom';
import { GoogleLogin } from 'react-google-login';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import { IDENTITY, ERROR } from '../../types/actionTypes';
import Input from "./Input";
import useStyles from './styles';
import { FormEvent } from 'react';
import Icon from "./icon";
import { signup, signin } from '../../actions/identity';
import IUser from '../../types/IUser';

const initialState: IUser = { 
    firstName: '', 
    lastName: '', 
    email: '', 
    password: '', 
    confirmPassword: '' 
};

const Identity = () => {
    const dispatch = useDispatch();
    const history = useHistory();
    const classes = useStyles();
    const [isSignUp, setIsSignUp] = useState(false);
    const [form, setForm] = useState(initialState);

    // const handleShowPassword = () => setShowPassword((previousState) => !previousState); this is how u get the prev state of useState element

    const switchMode = () => {
        setForm(initialState);
        setIsSignUp((prevIsSignup: boolean) => !prevIsSignup);
        // this is how u get the prev state of useState element
    };

    const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (isSignUp) {
            dispatch(signup(form, history));
        } else {
            dispatch(signin(form, history));
        }
    };

    const handleChange = (event: any) => {
        setForm({ ...form, [event.target.name]: event.target.value })
    };

    const googleSuccess = async (res: any) => {
        const result = res?.profileObj;
        const token = res?.tokenId;

        try {
            dispatch({ type: IDENTITY, data: { result, token } });

            history.push('/');
        } catch (error) {
            dispatch({ type: ERROR, error });
        }
    };

    const googleError = () => alert('Google Sign In was unsuccessful. Try again later');


    return (
        <Grow in>
            <Container component="main" maxWidth="xs">
                <Paper className={classes.paper} elevation={3}>
                    <Avatar className={classes.avatar}>
                        <LockOutlinedIcon />
                    </Avatar>
                    <Typography variant="h5">{isSignUp ? "Sign Up" : "Sign In"}</Typography>
                    <form className={classes.form} onSubmit={handleSubmit}>
                        <Grid container spacing={2}>
                            {isSignUp && (
                                <>
                                    <Input value={form.firstName} name="firstName" label="First Name" handleChange={handleChange} autoFocus half />
                                    <Input value={form.lastName} name="lastName" label="Last Name" handleChange={handleChange} half />
                                </>
                            )}
                            <Input value={form.email} name="email" label="Email Address" handleChange={handleChange} type="email" />
                            <Input value={form.password} name="password" label="Password" handleChange={handleChange} type="password" />
                            {isSignUp && <Input value={form.confirmPassword} name="confirmPassword" label="Repeat Password" handleChange={handleChange} type="password" />}
                        </Grid>
                        <Button type="submit" fullWidth variant="contained" color="primary" className={classes.submit}>
                            {isSignUp ? 'Sign Up' : 'Sign In'}
                        </Button>
                        <GoogleLogin
                            clientId="1069885603867-d7reqfc6oem96kqfvke545ckvol1d3de.apps.googleusercontent.com"
                            render={(renderProps) => (
                                <Button className={classes.googleButton} color="primary" fullWidth onClick={renderProps.onClick} disabled={renderProps.disabled} startIcon={<Icon />} variant="contained">
                                    Google Sign In
                                </Button>
                            )}
                            onSuccess={googleSuccess}
                            onFailure={googleError}
                            cookiePolicy="single_host_origin"
                        />
                        <Button onClick={switchMode} fullWidth variant="contained" color="secondary">
                            {isSignUp ? 'Already have an account? Sign in' : "Don't have an account? Sign Up"}
                        </Button>
                    </form>
                </Paper>
            </Container>

        </Grow>
    );
}

export default Identity;