import jwt, { decode } from "jsonwebtoken";
import dotenv from "dotenv";

dotenv.config();

const authorization = async (req, res, next) => {
    try {
        const token = req.headers.authorization.split(" ")[1];
        const isCustomIdentity = token.length < 500;

        let decodedData;

        if(token && isCustomIdentity) {
            decodedData = jwt.verify(token, process.env.JWT);

            req.userId = decodedData?.id;
        } else {
            decodedData = jwt.decode(token);

            req.userId = decodedData?.sub;
        }

        next();

    } catch (error) {
        console.log(error);
    }
}

export default authorization;