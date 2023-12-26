import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { AddBookComponent } from './components/add-book/add-book.component';
import { LentedBooksComponent } from './components/lented-books/lented-books.component';
import { BorrowedBooksComponent } from './components/borrowed-books/borrowed-books.component';
import { BookDetailsComponent } from './components/book-details/book-details.component';
import { authGuard } from './guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    title: 'BookBorrowing | Home'
  },
  {
    path: 'login',
    component: LoginComponent,
    title: 'BookBorrowing | Sign In'
  },
  {
    path: 'books/add',
    component: AddBookComponent,
    title: 'BookBorrowing | Add Book',
    canActivate: [authGuard]
  },
  {
    path: 'books/lented',
    component: LentedBooksComponent,
    title: 'BookBorrowing | Lented Books',
    canActivate: [authGuard]
  },
  {
    path: 'books/borrowed',
    component: BorrowedBooksComponent,
    title: 'BookBorrowing | Borrowed Books',
    canActivate: [authGuard]
  },
  {
    path: 'books/view/:id',
    component: BookDetailsComponent,
    title: 'BookBorrowing | Book Details'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
 }
