export class UserRegister{
    name: string
    email: string
    dateOfBirth:Date
    constructor(name: string, email: string, dateOfBirth:Date){
        this.name = name
        this.email = email
        this.dateOfBirth=dateOfBirth
    }
}
