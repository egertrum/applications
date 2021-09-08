import { Container, AppBar, Typography, Grow, Grid } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { BrowserRouter, Switch, Route } from "react-router-dom";
import { getPosts } from "./actions/posts";
import { useDispatch } from "react-redux";
import Navbar from "./components/Navbar/Navbar";
import Home from "./components/Home/Home";
import Identity from "./components/Identity/Identity";
import useStyles from "./styles";

function App() {
  const classes = useStyles();
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(getPosts());
  }, []);

  return (
    <BrowserRouter>
      <Container>
        <Navbar></Navbar>
        <Switch>
          <Route path="/" exact component={Home} />
          <Route path="/login" exact component={Identity} />
        </Switch>
      </Container>
    </BrowserRouter>
  );
}

export default App;