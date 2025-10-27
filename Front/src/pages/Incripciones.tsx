import { useForm } from 'react-hook-form'
import * as yup from 'yup'
import { yupResolver } from '@hookform/resolvers/yup'
import api from '../api'
import { useNotification } from '../notifications' 

const schema = yup.object({
  tipoDocumento: yup.string().required('El tipo de documento es requerido'),
  
  numeroDocumento: yup.string()
    .required('El número de documento es requerido')
    .matches(/^[0-9]+$/, 'El número de documento debe contener solo dígitos numéricos.'),
    
  nombresApellidos: yup.string()
    .required('Los nombres y apellidos son requeridos')
    .matches(/^[a-zA-Z\s]+$/, 'El nombre solo puede contener letras y espacios.'),
    
  fechaNacimiento: yup.date().required('La fecha de nacimiento es requerida'),
  direccion: yup.string().required('La dirección es requerida'),
  
  telefono: yup.string()
    .required('El teléfono es requerido')
    .matches(/^[0-9]+$/, 'El teléfono debe contener solo dígitos numéricos.'),
    
  correo: yup.string().email('Debe ser un correo electrónico válido').required('El correo es requerido'),
  documento: yup.mixed().required('Adjunta un PDF o imagen')
})

export default function Inscripciones() {

  const { register, handleSubmit, formState: { errors }, reset } = useForm({
    resolver: yupResolver(schema)
  })

  const { showNotification } = useNotification()

  const onSubmit = async (data: any) => {
    try {
      const form = new FormData()

      form.append('tipoDocumento', data.tipoDocumento)
      form.append('numeroDocumento', data.numeroDocumento)
      form.append('nombresApellidos', data.nombresApellidos)
      form.append('direccion', data.direccion)
      form.append('telefono', data.telefono)
      form.append('correo', data.correo)

      if (data.fechaNacimiento) {
        const dateObj = new Date(data.fechaNacimiento);

        const datePart = dateObj.toISOString().slice(0, 10);
        const dateValue = datePart + 'T00:00:00';
        form.append('fechaNacimiento', dateValue);
      }

      if (data.documento && data.documento[0]) {
        form.set('Documento', data.documento[0])
      }

      await api.post('/api/inscripciones', form, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      })

      showNotification('✅ Inscripción realizada exitosamente. Muchas gracias', 'success')
      reset()

    } catch (e: any) {
      console.error("Error completo de Axios:", e)

      let errorMessage = 'Error al enviar la inscripción. Verifique los campos.'
      if (e.response && e.response.data) {
        const errorData = e.response.data;

        if (Array.isArray(errorData)) {
          errorMessage = "Errores de validación: " + errorData.join(', ')
        } else if (errorData.errors) {
          const errorMessages = Object.values(errorData.errors).flat();
          errorMessage = (errorMessages[0] as string) || errorMessage;
        } else if (typeof errorData === 'string') {
          errorMessage = errorData;
        }
      }
      showNotification(`❌ ${errorMessage}`, 'error')
    }
  }

  return (
    <div className='flex items-center justify-center min-h-screen bg-gray-100 p-4'>
      <div className='bg-white shadow-xl rounded-lg w-full max-w-xl p-6'>
        <h2 className='text-2xl font-bold text-center mb-6 text-indigo-600'>Formulario de Inscripción</h2>
        <form onSubmit={handleSubmit(onSubmit)} className='grid grid-cols-1 md:grid-cols-2 gap-5'>

          <div className='col-span-2 md:col-span-1'>
            <label className='block text-sm font-medium text-gray-700 mb-1'>Tipo documento *</label>
            <div className='relative'>
              <select
                {...register('tipoDocumento')}
                className='w-full p-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 bg-white appearance-none cursor-pointer pr-10'
              >
                <option value="">Seleccione un tipo</option>
                <option value="CC">Cédula de Ciudadanía (CC)</option>
                <option value="CE">Cédula de Extranjería (CE)</option>
                <option value="TI">Tarjeta de Identidad (TI)</option>
                <option value="RC">Registro Civil (RC)</option>
                <option value="PA">Pasaporte (PA)</option>
                <option value="PEP">Permiso Especial de Permanencia (PEP)</option>
                <option value="PPT">Permiso Por Protección Temporal (PPT)</option>
              </select>
   
              <div className='pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700'>
                <svg className='w-4 h-4' fill='none' stroke='currentColor' viewBox='0 0 24 24'>
                  <path strokeLinecap='round' strokeLinejoin='round' strokeWidth='2' d='M19 9l-7 7-7-7' />
                </svg>
              </div>
            </div>
            {errors.tipoDocumento && <p className='text-red-500 text-xs mt-1'>{errors.tipoDocumento.message}</p>}
          </div>

          <div className='col-span-2 md:col-span-1'>
            <label className='block text-sm font-medium text-gray-700 mb-1'>Número documento *</label>
            <input
              {...register('numeroDocumento')}
              className='w-full p-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 bg-white'
              placeholder="Ej: 123456789"
            />
            {errors.numeroDocumento && <p className='text-red-500 text-xs mt-1'>{errors.numeroDocumento.message}</p>}
          </div>

          <div className='col-span-2'>
            <label className='block text-sm font-medium text-gray-700 mb-1'>Nombres y apellidos completos *</label>
            <input
              {...register('nombresApellidos')}
              className='w-full p-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 bg-white'
              placeholder="Ej: María González Pérez"
            />
            {errors.nombresApellidos && <p className='text-red-500 text-xs mt-1'>{errors.nombresApellidos.message}</p>}
          </div>

          <div className='col-span-2 md:col-span-1'>
            <label className='block text-sm font-medium text-gray-700 mb-1'>Fecha de nacimiento *</label>
            <input
              type='date'
              {...register('fechaNacimiento')}
              className='w-full p-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 bg-white'
            />
            {errors.fechaNacimiento && <p className='text-red-500 text-xs mt-1'>La fecha es requerida y debe ser válida.</p>}
          </div>

          <div className='col-span-2 md:col-span-1'>
            <label className='block text-sm font-medium text-gray-700 mb-1'>Teléfono *</label>
            <input
              {...register('telefono')}
              className='w-full p-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 bg-white'
              placeholder="Ej: 3001234567"
            />
            {errors.telefono && <p className='text-red-500 text-xs mt-1'>{errors.telefono.message}</p>}
          </div>

          <div className='col-span-2'>
            <label className='block text-sm font-medium text-gray-700 mb-1'>Dirección completa *</label>
            <input
              {...register('direccion')}
              className='w-full p-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 bg-white'
              placeholder="Ej: Calle 123 #45-67, Ciudad"
            />
            {errors.direccion && <p className='text-red-500 text-xs mt-1'>{errors.direccion.message}</p>}
          </div>

          <div className='col-span-2'>
            <label className='block text-sm font-medium text-gray-700 mb-1'>Correo electrónico *</label>
            <input
              type='email'
              {...register('correo')}
              className='w-full p-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 bg-white'
              placeholder="Ej: ejemplo@correo.com"
            />
            {errors.correo && <p className='text-red-500 text-xs mt-1'>{errors.correo.message}</p>}
          </div>

          <div className='col-span-2'>
            <label className='block text-sm font-medium text-gray-700 mb-1'>Documento Adjunto (PDF o Imagen)</label>
            <input type='file' {...register('documento')} className='w-full p-2 border border-gray-300 rounded-lg file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-indigo-50 file:text-indigo-700 hover:file:bg-indigo-100' />
            {errors.documento && <p className='text-red-500 text-xs mt-1'>{errors.documento.message}</p>}
          </div>

          <button
            type='submit'
            className='col-span-2 mt-6 bg-indigo-600 hover:bg-indigo-700 text-white font-semibold py-3 rounded-lg transition duration-150 ease-in-out shadow-lg transform hover:scale-[1.01]'
          >
            Enviar Inscripción
          </button>
        </form>
      </div>
    </div>
  )
}
