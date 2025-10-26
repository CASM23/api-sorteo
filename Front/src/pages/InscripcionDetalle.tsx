import { useParams, useNavigate } from 'react-router-dom';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../api';

export default function InscripcionDetalle(){
  const { id } = useParams()
  const qc = useQueryClient()
  const nav = useNavigate()

  const { data, isLoading } = useQuery({
    queryKey: ['inscripcion', id],
    queryFn: async () => {
      const res = await api.get(`/api/admin/inscripciones/${id}`)
      return res.data
    },
    enabled: !!id
  })

  const mutation = useMutation({
    mutationFn: async (estado: string) => {
      await api.put(`/api/admin/inscripciones/${id}/estado`, { Estado: estado })
    },
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['inscripciones'] });
      nav('/admin');
    }
  })

  if(isLoading) return (
    <div className='p-6 bg-gray-50 min-h-screen flex items-center justify-center'>
      <div className='text-center'>
        <div className='animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600 mx-auto mb-4'></div>
        <p className='text-lg text-indigo-600'>Cargando detalles...</p>
      </div>
    </div>
  )
  
  if(!data) return (
    <div className='p-6 bg-gray-50 min-h-screen flex items-center justify-center'>
      <div className='text-center'>
        <div className='text-6xl mb-4'>😞</div>
        <h2 className='text-2xl font-bold text-gray-800 mb-2'>No encontrado</h2>
        <p className='text-gray-600 mb-4'>La inscripción solicitada no existe o ha sido eliminada.</p>
        <button 
          onClick={() => nav('/admin')}
          className='bg-indigo-600 hover:bg-indigo-700 text-white font-semibold py-2 px-4 rounded-lg transition duration-150'
        >
          Volver al listado
        </button>
      </div>
    </div>
  )

  return (
    <div className='p-6 bg-gray-50 min-h-screen'>
      <div className='max-w-4xl mx-auto'>
        
        {/* Encabezado con navegación */}
        <div className='flex items-center justify-between mb-6 p-4 bg-white shadow rounded-lg'>
          <div className='flex items-center space-x-4'>
            <button 
              onClick={() => nav('/admin')}
              className='bg-gray-500 hover:bg-gray-600 text-white font-semibold py-2 px-4 rounded-lg transition duration-150 shadow-md'
            >
              ← Volver
            </button>
            <h1 className='text-3xl font-bold text-indigo-700'>Detalle de Inscripción</h1>
          </div>
        </div>

        {/* Información principal */}
        <div className='bg-white shadow-xl rounded-lg p-6 mb-6'>
          <div className='flex items-center justify-between mb-6'>
            <div>
              <h2 className='text-2xl font-bold text-gray-800 mb-2'>{data.nombresApellidos}</h2>
              <div className='text-sm text-gray-500'>
                Registrado: {new Date(data.fechaRegistro).toLocaleString()}
              </div>
            </div>
            <div className='text-right'>
              <span className={`px-3 py-1 rounded-full text-sm font-semibold ${
                data.estado === 'Aceptada' ? 'bg-green-100 text-green-800' :
                data.estado === 'Rechazada' ? 'bg-red-100 text-red-800' :
                'bg-yellow-100 text-yellow-800'
              }`}>
                {data.estado}
              </span>
            </div>
          </div>

          {/* Información detallada */}
          <div className='grid grid-cols-1 md:grid-cols-2 gap-6 mb-6'>
            <div className='space-y-4'>
              <div className='bg-gray-50 p-4 rounded-lg'>
                <label className='block text-sm font-medium text-gray-700 mb-1'>Documento de Identidad</label>
                <p className='text-gray-900'>{data.tipoDocumento} - {data.numeroDocumento}</p>
              </div>
              
              <div className='bg-gray-50 p-4 rounded-lg'>
                <label className='block text-sm font-medium text-gray-700 mb-1'>Fecha de Nacimiento</label>
                <p className='text-gray-900'>{new Date(data.fechaNacimiento).toLocaleDateString()}</p>
              </div>
              
              <div className='bg-gray-50 p-4 rounded-lg'>
                <label className='block text-sm font-medium text-gray-700 mb-1'>Dirección</label>
                <p className='text-gray-900'>{data.direccion}</p>
              </div>
            </div>
            
            <div className='space-y-4'>
              <div className='bg-gray-50 p-4 rounded-lg'>
                <label className='block text-sm font-medium text-gray-700 mb-1'>Teléfono</label>
                <p className='text-gray-900'>{data.telefono}</p>
              </div>
              
              <div className='bg-gray-50 p-4 rounded-lg'>
                <label className='block text-sm font-medium text-gray-700 mb-1'>Correo Electrónico</label>
                <p className='text-gray-900'>{data.correo}</p>
              </div>
            </div>
          </div>

          {/* Documento adjunto */}
          {data.documentoUrl && (
            <div className='bg-indigo-50 p-4 rounded-lg mb-6'>
              <label className='block text-sm font-medium text-gray-700 mb-2'>Documento Adjunto</label>
              <a 
                href={`${(import.meta as any).env.VITE_API_BASE || 'http://localhost:5184'}${data.documentoUrl}`} 
                target='_blank' 
                rel='noreferrer' 
                className='inline-flex items-center px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white font-semibold rounded-lg transition duration-150 shadow-md'
              >
                📄 Ver Documento
              </a>
            </div>
          )}

          <div className='flex gap-4 justify-end'>
            <button 
              className='px-6 py-3 bg-green-600 hover:bg-green-700 text-white font-semibold rounded-lg transition duration-150 shadow-lg transform hover:scale-[1.02]'
              onClick={() => mutation.mutate('Aceptada')}
              disabled={mutation.isPending}
            >
              {mutation.isPending ? 'Procesando...' : '✓ Aceptar'}
            </button>
            <button 
              className='px-6 py-3 bg-red-600 hover:bg-red-700 text-white font-semibold rounded-lg transition duration-150 shadow-lg transform hover:scale-[1.02]'
              onClick={() => mutation.mutate('Rechazada')}
              disabled={mutation.isPending}
            >
              {mutation.isPending ? 'Procesando...' : '✗ Rechazar'}
            </button>
          </div>
        </div>
      </div>
    </div>
  )
}