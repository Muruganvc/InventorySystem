import { CommonModule } from '@angular/common';
import { Component, computed, inject } from '@angular/core';
import { Loader } from '../../../services/loader';

@Component({
  selector: 'app-spinner',
  imports: [CommonModule],
  templateUrl: './spinner.html',
  styleUrl: './spinner.scss'
})
export class Spinner {
loader = inject(Loader);
  visible = this.loader.showSignal;
}
