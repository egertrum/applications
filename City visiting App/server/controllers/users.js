import bcrypt from "bcryptjs"; 
// hashing passwords
import jwt from "jsonwebtoken";
import User from "../models/user.js";
import dotenv from "dotenv";

dotenv.config();

export const signIn = async (req, res) => {
    const { email, password } = req.body;

    try {
        const user = await User.findOne({ email });
        if(!user) return res.status(404).json({ message: "User does not exist!" });

        const isPasswordCorrect = await bcrypt.compare(password, user.password);
        if(!isPasswordCorrect) return res.status(400).json({ message: "Invalid credentials!" });

        const token = jwt.sign({ email: user.email, id: user._id }, process.env.JWT, { expiresIn: "1h" });

        res.status(200).json({ result: user, token: token })
    } catch(error) {
        res.status(500).json({ message: "Something went wrong!" });
    }
}

export const signUp = async (req, res) => {
    const { email, password, confirmPassword, firstName, lastName } = req.body;

    try {
        const user = await User.findOne({ email });
        if(user) return res.status(404).json({ message: "User already exists!" });

        const hashedPassword = await bcrypt.hash(password, 12);

        const result = await User.create({ email, password: hashedPassword, firstName, lastName });

        const token = jwt.sign({ email, id: result._id }, process.env.JWT, { expiresIn: "1h" });

        res.status(200).json({ result: result, token: token });
    } catch(error) {
        res.status(500).json({ message: "Something went wrong!" });
    }
}

export const getUser = async (req, res) => {
    const { id } = req.params;

    try {
        const user = await User.findOne({ _id: id });
        if(!user) return res.status(404).json({ message: "User does not exist!" });

        res.status(200).json({ result: user });
    } catch(error) {
        res.status(500).json({ message: "Something went wrong!" });
    }
}