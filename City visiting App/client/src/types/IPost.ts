export default interface IPost {
    _id?: String,
    title: String,
    message: String,
    creatorId?: String,
    name?: String,
    tags: String,
    selectedFile: String,
    likes?: [String] | null,
    createdAt?: Date;
}