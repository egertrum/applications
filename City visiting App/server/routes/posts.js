import express from "express";
import { getPosts, createPost, getPost, updatePost, deletePost, likePost } from "../controllers/posts.js";
import authorization from "../middleware/authorization.js";

const router = express.Router();

router.get("/", getPosts);
router.get('/:id', getPost);
router.post("/", authorization, createPost);
router.patch('/:id', authorization, updatePost);
router.delete('/:id', authorization, deletePost);
router.patch('/:id/like', authorization, likePost);

export default router;