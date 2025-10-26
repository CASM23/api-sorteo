import React, { createContext, useContext, useEffect, useState } from 'react'
import api, { setAuthToken } from '../api'

type User = { nombreUsuario: string; rol: string }

const AuthContext = createContext<any>(null)

export const useAuthProvider = () => useContext(AuthContext)

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(() => {
    const s = localStorage.getItem('syc_user')
    return s ? JSON.parse(s) : null
  })

  useEffect(() => {
    const token = localStorage.getItem('syc_token')
    setAuthToken(token || undefined)
  }, [])

  const login = (data: any) => {
    localStorage.setItem('syc_token', data.token)
    localStorage.setItem('syc_user', JSON.stringify({ nombreUsuario: data.nombreUsuario, rol: data.rol }))
    setAuthToken(data.token)
    setUser({ nombreUsuario: data.nombreUsuario, rol: data.rol })
  }

  const logout = () => {
    localStorage.removeItem('syc_token')
    localStorage.removeItem('syc_user')
    setAuthToken(undefined)
    setUser(null)
  }

  return <AuthContext.Provider value={{ user, login, logout }}>{children}</AuthContext.Provider>
}
