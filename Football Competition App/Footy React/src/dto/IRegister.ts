export interface IRegister {
    email: string | null,
    password: string,
    confirmPassword?: string,
    firstName: string,
    lastName: string,
    identificationCode: string,
    birthDate: Date | string,
    gender?: string,
    countryId: string
}