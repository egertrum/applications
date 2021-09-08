import express from "express";
import { signIn, signUp, getUser } from "../controllers/users.js";

const router = express.Router();

router.post("/signin", signIn);
router.post("/signup", signUp);
router.get('/:id', getUser);

export default router;