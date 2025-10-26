import React from 'react'
import { Routes, Route, Navigate } from 'react-router-dom'
import HomePage from './pages/HomePage'
import LoginPage from './pages/Login'
import Inscripciones from './pages/Incripciones'
import AdminListPage from './pages/AdminList'
import InscripcionDetalle from './pages/InscripcionDetalle'
import { useAuthProvider, AuthProvider } from './provider/auth'

const PrivateRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const { user } = useAuthProvider()
  if (!user) return <Navigate to='/login' />
  if (user.rol !== 'Admin') return <Navigate to='/login' />
  return <>{children}</>
}

export default function App() {
  return (
    <AuthProvider>
      <div className='min-h-screen flex flex-col'>
        <Routes>
          <Route path='/' element={<HomePage />} />
          <Route path='/login' element={<LoginPage />} />
          <Route path='/inscripcion' element={<Inscripciones />} />

          <Route path='/admin' element={<PrivateRoute><AdminListPage /></PrivateRoute>} />
          <Route path='/admin/inscripcion/:id' element={<PrivateRoute><InscripcionDetalle /></PrivateRoute>} />
        </Routes>
      </div>
    </AuthProvider>
  )
}
