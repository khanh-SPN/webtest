import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { HarvestService } from '../services/harvest-service';

// The API rejects empty or whitespace-only values, so the form does too.
const requiredText = [Validators.required, Validators.pattern(/\S/)];

@Component({
  selector: 'app-harvest-add-33603294',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './harvest-add-33603294.html',
  styleUrl: './harvest-add-33603294.css',
})
export class HarvestAdd33603294 {
  private readonly harvestService = inject(HarvestService);
  private readonly router = inject(Router);

  protected readonly form = new FormGroup({
    variety: new FormControl('', { nonNullable: true, validators: requiredText }),
    harvestedBy: new FormControl('', { nonNullable: true, validators: requiredText }),
    harvestDate: new FormControl('', { nonNullable: true, validators: requiredText }),
    notes: new FormControl('', { nonNullable: true, validators: requiredText }),
  });

  protected readonly saving = signal(false);
  protected readonly errorMessage = signal('');

  protected showError(field: keyof typeof this.form.controls): boolean {
    const control = this.form.controls[field];
    return control.invalid && control.touched;
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    this.errorMessage.set('');
    this.harvestService.add(this.form.getRawValue()).subscribe({
      next: () => this.router.navigate(['/33603294/harvests']),
      error: (err) => {
        this.saving.set(false);
        this.errorMessage.set(err.error?.error ?? 'Could not add the harvest.');
      },
    });
  }
}
