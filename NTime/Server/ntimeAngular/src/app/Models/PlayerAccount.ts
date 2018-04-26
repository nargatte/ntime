export class PlayerAccount {
    constructor (
        public Id: number,
        public FirstName: string,
        public LastName: string,
        public BirthDate: Date,
        public IsMale: Boolean,
        public Team: string,
        public PhoneNumber: string,
        public EMail: string,
        public City: string
    ) {}
}
