import React from "react";
import useStyles from "./styles";
import { Card, CardActions, CardContent, CardMedia, Button, Typography } from "@material-ui/core";
import ThumbUpAltIcon from "@material-ui/icons/ThumbUpAlt";
import DeleteIcon from "@material-ui/icons/Delete"
import MoreHorizIcon from '@material-ui/icons/MoreHoriz';
import moment from 'moment';
import { useDispatch } from 'react-redux';
import { deletePost, likePost } from "../../../actions/posts";

const Post = ({ post, setCurrentId }: any) => {
  const classes = useStyles();
  const dispatch = useDispatch();
  const user = localStorage.getItem("profile") === null ? null : JSON.parse(localStorage.getItem("profile")!);

  const deleteOnePost = async () => {
    dispatch(deletePost(post._id));
  };

  const likeOnePost = async () => {
    dispatch(likePost(post._id));
  };
  //console.log(post);

  return (
    <Card className={classes.card}>
      <CardMedia className={classes.media} image={post.selectedFile || 'https://user-images.githubusercontent.com/194400/49531010-48dad180-f8b1-11e8-8d89-1e61320e1d82.png'} title={post.title} />
      <div className={classes.overlay}>
        <Typography variant="h6">{post.title}</Typography>
        <Typography variant="body2">{moment(post.createdAt).fromNow()}</Typography>
      </div>
      <div className={classes.overlay2}>
        <Button style={{ color: 'white' }} size="small" onClick={() => setCurrentId(post._id)}><MoreHorizIcon fontSize="medium" /></Button>
      </div>
      <div className={classes.details}>
        <Typography variant="body2" color="textSecondary" component="h2">{post.tags.map((tag: any) => `#${tag} `)}</Typography>
      </div>
      <Typography className={classes.title} gutterBottom component="p">{post.message}</Typography>
      <CardContent>
        <Typography variant="body2" color="textSecondary" component="p">@{post.name}</Typography>
      </CardContent>
      <CardActions className={classes.cardActions}>
        <Button size="small" disabled={!user} color="primary" onClick={() => { likeOnePost() }}>
          <ThumbUpAltIcon fontSize="small" /> Like {post.likes.length}
        </Button>
        {(user?.result?.googleId === post?.creatorId || user?.result?._id === post?.creatorId) && (
          <Button size="small" color="primary" onClick={() => { deleteOnePost() }}>
          <DeleteIcon fontSize="small" /> Delete
        </Button>
        )}
      </CardActions>
    </Card>
  );
}

export default Post;