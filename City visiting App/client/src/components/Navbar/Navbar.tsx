import { useState, useEffect } from "react";
import {  AppBar, Typography, Grow, Grid, Toolbar, Avatar, Button } from "@material-ui/core";
import { Link, useHistory, useLocation } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import useStyles from "./styles";
import decode from 'jwt-decode';
import visit from "../../images/map.png";
import * as actionType from '../../types/actionTypes';

const Navbar = () => {
    const [user, setUser] = useState(JSON.parse(localStorage.getItem('profile')!));
    const classes = useStyles();
    const history = useHistory();
    const dispatch = useDispatch();
    const authentication = useSelector((state: any) => state.identity);

    const logout = () => {
        dispatch({ type: actionType.LOGOUT });

        history.push('/login');

        setUser(null);
    };

    useEffect(() => {
        const token = user?.token;

        if (token) {
            const decodedToken: any = decode(token);

            if (decodedToken.exp * 1000 < new Date().getTime()) logout();
        }

        setUser(JSON.parse(localStorage.getItem('profile')!));
        //{user?.result.firstName.charAt(0).toUpperCase()}
    }, [authentication])

    return (
        <div>
            <AppBar className={classes.appBar} position="static" color="inherit">
                <div className={classes.brandContainer}>
                    <Typography component={Link} to="/" className={classes.heading} variant="h3" align="center">
                        Visit Tartu
                    </Typography>
                    <img className={classes.image} src={visit} alt="Visit" height="50" />
                </div>
                <Toolbar className={classes.toolbar}>
                    {user?.result ? (
                        <div className={classes.profile}>
                            <Avatar className={classes.green} alt={user?.result.name} src={user?.result.imageUrl}></Avatar>
                            <Typography className={classes.userName} variant="h6">{user?.result.name}</Typography>
                            <Button variant="contained" color="secondary" onClick={logout}>Logout</Button>
                        </div>
                    ) : (
                        <Button component={Link} to="/login" variant="contained" color="primary">Sign In</Button>
                    )}
                </Toolbar>
            </AppBar>
        </div>
    )
}

export default Navbar;