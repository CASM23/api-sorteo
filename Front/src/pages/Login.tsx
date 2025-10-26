import { useForm } from 'react-hook-form'
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from 'yup'
import api from '../api'
import { useAuthProvider } from '../providers/auth'
import { useNavigate } from 'react-router-dom'
import { useNotification } from '../notifications'



const loginSchema = yup.object({
  nombreUsuario: yup.string().required('Usuario es requerido'),
  clave: yup.string().required('Clave es requerida')
}).required()

export default function LoginPage () {
  const { register, handleSubmit, formState:{ errors } } = useForm({ resolver: yupResolver(loginSchema) })
  const { login } = useAuthProvider()
  const nav = useNavigate()
  const { showNotification } = useNotification()

  const onSubmit = async (data: any) => {
    try{

      const res = await api.post('/api/auth/login', data)
      login(res.data) 
      nav('/admin')
    }catch(e: any){
      console.error("Error en login:", e);
      showNotification(`❌ ${e.response?.data?.message || 'Error en login. Revise sus credenciales.'}`, 'error')
    }
  }

  return (
    <div className='flex items-center justify-center min-h-screen bg-gray-100 p-4'>
      <div className='bg-white shadow-xl rounded-lg w-full max-w-md p-6'>
        <h2 className='text-2xl font-bold text-center mb-6 text-indigo-600'>Iniciar Sesión - SYC</h2>
        <form onSubmit={handleSubmit(onSubmit)} className='space-y-5'>
          
          <div>
            <label className='block text-sm font-medium text-gray-700 mb-1'>Usuario</label>
            <input 
              {...register('nombreUsuario')} 
              className='w-full p-2 border border-gray-300 rounded-lg focus:ring-indigo-500 focus:border-indigo-500' 
            />
            <p className='text-red-500 text-xs mt-1'>{errors.nombreUsuario?.message}</p>
          </div>

          <div>
            <label className='block text-sm font-medium text-gray-700 mb-1'>Clave</label>
            <input 
              type='password' 
              {...register('clave')} 
              className='w-full p-2 border border-gray-300 rounded-lg focus:ring-indigo-500 focus:border-indigo-500' 
            />
            <p className='text-red-500 text-xs mt-1'>{errors.clave?.message}</p>
          </div>

          <button 
            type='submit' 
            className='w-full mt-6 bg-indigo-600 hover:bg-indigo-700 text-white font-semibold py-3 rounded-lg transition duration-150 ease-in-out shadow-lg transform hover:scale-[1.01]'
          >
            Entrar
          </button>
        </form>
      </div>
    </div>
  )
}