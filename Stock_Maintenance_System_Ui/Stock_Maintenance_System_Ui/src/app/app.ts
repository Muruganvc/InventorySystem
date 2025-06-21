import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormGroup, FormControl } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { Spinner } from "./shared/components/spinner/spinner";
import { LoaderInterceptor } from './Interceptors/LoaderInterceptor';
import { HttpClient } from '@angular/common/http';
import { APP_CONFIG } from './shared/common/app-config';
import { ToastrService } from 'ngx-toastr';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MatToolbarModule, MatButtonModule, MatIconModule,CommonModule, NgSelectModule, ReactiveFormsModule, CommonModule, FormsModule, Spinner],
  templateUrl: './app.html',
  styleUrl: './app.scss'

})
export class App implements OnInit {
  cities = [
    { id: 1, name: 'Chennai' },
    { id: 2, name: 'Mumbai' },
    { id: 3, name: 'Delhi' }
  ];

  form!: FormGroup;
  private http = inject(HttpClient);
  private toastr = inject(ToastrService);
  private readonly config = inject(APP_CONFIG);

  data = signal<any>(null);

  ngOnInit() {
    this.form = new FormGroup({
      selectedCity: new FormControl(5),
    });
  }

  fetchData() {
    this.toastr.success('Data saved successfully!', 'Success');
    this.toastr.error('Something went wrong.', 'Error');
    this.toastr.info('This is an info message.', 'Information');
    this.toastr.warning('This is a warning!', 'Warning');
    console.log(this.config.apiBaseUrl);
    this.http
      .get('https://jsonplaceholder.typicode.com/posts?_limit=50000')
      .subscribe((res) => this.data.set(res));
  }
}