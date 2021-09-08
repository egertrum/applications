import React, { useEffect, useState } from "react";
import useStyles from "./styles";
import { TextField, Button, Typography, Paper } from "@material-ui/core";
//import FileBase from "react-file-base64";
import { useDispatch, useSelector } from "react-redux";
import { createPost, updatePost } from "../../actions/posts";

const Form = ({ currentId, setCurrentId }: any) => {
    const classes = useStyles();
    const dispatch = useDispatch();
    const post = useSelector((state: any) => (currentId ? state.posts.find((message: any) => message._id === currentId) : null));
    const user = JSON.parse(localStorage.getItem("profile")!);

    const [postData, setPostData] = useState({
        title: "",
        message: "",
        name: "",
        tags: "",
        selectedFile: ""
    });

    useEffect(() => {
        if (post) setPostData(post);
    }, [post]);
    // [] -> argument, mis utleb millal peaks effecti runima, ehk kui post value muutub

    const uploadImage = async (e: any) => {
        const file = e.target.files[0];
        const base64 = await convertBase64(file);
        setPostData({ ...postData, selectedFile: base64 as unknown as string });
    };

    const convertBase64 = (file: any) => {
        return new Promise((resolve, reject) => {
            const fileReader = new FileReader();
            fileReader.readAsDataURL(file);

            fileReader.onload = () => {
                resolve(fileReader.result);
            };

            fileReader.onerror = (error) => {
                reject(error);
            };
        });
    };

    const handleSubmit = async (e: any) => {
        e.preventDefault();

        let name;
        if (user.result?.googleId) {
            name = user.result.name;
        } else {
            name = user.result.firstName + " " + user.result.lastName;
        }

        if (currentId === 0) {
            dispatch(createPost({ ...postData, name: name }));
        } else {
            dispatch(updatePost(currentId, { ...postData, name: name }));
        }
        clear();
    };

    const clear = () => {
        setCurrentId(0);
        setPostData({ title: '', message: '', name:'', tags: '', selectedFile: '' });
    };

    if (!user?.result) {
        return (
            <Paper className={classes.paper}>
                <Typography variant="h6" align="center">
                    Sign In to add places to visit!
                </Typography>
            </Paper>
        )
    }

    return (
        <Paper className={classes.paper}>
            <form autoComplete="off" noValidate className={classes.form} onSubmit={handleSubmit}>
                <Typography variant="h6">
                    {currentId ? "Edit the visiting place" : "Add a place to Visit"}
                </Typography>
                <TextField
                    name="title"
                    variant="outlined"
                    label="Title"
                    fullWidth
                    value={postData.title}
                    onChange={(e) => setPostData({ ...postData, title: e.target.value })}>
                </TextField>
                <TextField
                    name="message"
                    variant="outlined"
                    label="Message"
                    fullWidth
                    value={postData.message}
                    onChange={(e) => setPostData({ ...postData, message: e.target.value })}>
                </TextField>
                <TextField
                    name="tags"
                    variant="outlined"
                    label="Tags"
                    fullWidth
                    value={postData.tags}
                    onChange={(e) => setPostData({ ...postData, tags: e.target.value })}>
                </TextField>
                <div className={classes.fileInput}>
                    <input
                        type="file"
                        onChange={(e) => {
                            uploadImage(e);
                        }}
                        accept="image/png, image/gif, image/jpeg"
                    />
                </div>
                <Button className={classes.buttonSubmit} variant="contained" color="primary" size="large" type="submit" fullWidth>
                    {currentId ? "Edit" : "Submit"}
                </Button>
                <Button variant="contained" color="secondary" size="small" onClick={clear} fullWidth>
                    Clear
                </Button>
            </form>
        </Paper>
    );
}

export default Form;