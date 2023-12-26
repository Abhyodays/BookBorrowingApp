import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Book } from 'src/app/models/book.model';
import { AuthService } from 'src/app/services/auth.service';
import { BooksService } from 'src/app/services/books.service';

@Component({
  selector: 'app-add-book',
  templateUrl: './add-book.component.html',
  styleUrls: ['./add-book.component.css']
})
export class AddBookComponent {
  bookForm !: FormGroup;
  errors: string[] = [];
  userEmail!:string;
  constructor(
    private router: Router,
    private booksService: BooksService,
    private formBuilder: FormBuilder,
    private authService: AuthService
  ) {}
  ngOnInit() {
    this.initializeForm();
    this.authService.getTokenChange().subscribe((token:string|null) =>{ 
      const decodedToken = this.authService.decodeToken(token);
      this.userEmail = decodedToken.email;
    })
  }

  initializeForm() {
    this.bookForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(150)]],
      rating: ['', [Validators.required, Validators.min(0), Validators.max(5)]],
      author: ['', [Validators.required,Validators.maxLength(100)]],
      genre: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required]],
    });
  }
  save() {
    if (this.bookForm.invalid) {
      this.errors = [];
      if (this.bookForm.get('name')?.hasError('required')) {
        this.errors = [...this.errors, 'Name is required field'];
      }
      if (this.bookForm.get('rating')?.hasError('required')) {
        this.errors = [...this.errors, 'Rating is required field'];
      }
      if (this.bookForm.get('author')?.hasError('required')) {
        this.errors = [...this.errors, 'Author is required field'];
      }
      if (this.bookForm.get('genre')?.hasError('required')) {
        this.errors = [...this.errors, 'Genre is required field'];
      }
      if (this.bookForm.get('description')?.hasError('required')) {
        this.errors = [...this.errors, 'Description is required field'];
      }
      if (this.bookForm.get('name')?.hasError('maxLength')) {
        this.errors = [...this.errors, 'Name can have maximum 150 characters'];
      }
      if (this.bookForm.get('author')?.hasError('maxLength')) {
        this.errors = [...this.errors, 'Author can have maximum 100 characters'];
      }
      if (this.bookForm.get('genre')?.hasError('maxLength')) {
        this.errors = [...this.errors, 'Genre can have maximum 100 characters'];
      }
      if (this.bookForm.get('rating')?.hasError('min') || this.bookForm.get('rating')?.hasError('max')) {
        this.errors = [...this.errors, 'Rating must be between 0 and 5'];
      }
      return;
    }
    this.errors = [];
    const newBook: Book = {
      name: this.bookForm.value.name,
      author: this.bookForm.value.author,
      rating: this.bookForm.value.rating,
      genre: this.bookForm.value.genre,
      description: this.bookForm.value.description,
      lentByUserId: this.userEmail
    };
    this.booksService.addBook(newBook).subscribe({
      next: (res) => {
        // console.log('book added: ', res);
        this.router.navigate(['']);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
  cancel() {
    this.router.navigate(['']);
  }

}
