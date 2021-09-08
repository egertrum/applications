export interface IReport {
    id?: string | null,
    title: string,
    comment: string,
    appUserId?: string | null, 
    submitter?: string | null,
    date?: string | null
}