import { Competition } from '../Models/Competition';

export const COMPETITIONS: Competition[] = [
    new Competition(1, "Łask", new Date(Date.now())),
    new Competition(2, "Warszawa", new Date(2018, 8, 25)),
    new Competition(3, "Szczecin", new Date(2018, 9, 13)),
    //{ Id: 1, City: "Łask", EventDate: new Date(Date.now()), Link: },
    //{Id: 2, City: "Warszawa", EventDate: new Date(Date.now().toString()) },
    //{Id: 3, City: "Szczecin", EventDate: new Date(2018, 9, 13) },
]