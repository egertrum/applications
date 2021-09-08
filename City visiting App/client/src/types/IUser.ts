export default interface IUser {
    _id?: String,
    firstName: String,
    lastName: String,
    confirmPassword?: String | null | undefined
    password: String,
    email: String
}