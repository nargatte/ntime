import { Component } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule} from '@angular/forms'

//import { Competition } from '../../Models/Competition';

@Component({
    selector: 'app-competitions-select',
    templateUrl: './competitions-select.component.html',
    styleUrls: ['./competitions-select.component.css']
})
export class CompetitionsSelectComponent {
    animalControl = new FormControl('', [Validators.required]);

    animals = [
        { name: 'Dog', sound: 'Woof!' },
        { name: 'Cat', sound: 'Meow!' },
        { name: 'Cow', sound: 'Moo!' },
        { name: 'Fox', sound: 'Wa-pa-pa-pa-pa-pa-pow!' },
    ];

}
