
import { Link, useNavigate } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { useAuthProvider } from '../providers/auth';
import api from '../api';

type InscripcionItem = { id:number; nombresApellidos:string; fechaRegistro:string; }

const fetchInscripciones = async () => {
  const response = await api.get('/api/admin/inscripciones'); 
  return response.data as InscripcionItem[];
};

const AdminListPage = () => {

  const { user, logout } = useAuthProvider(); 
  const nav = useNavigate()

  const handleLogout = () => {
    logout(); 
    nav('/'); 
  };
  
  const { data: inscripciones, isLoading, error } = useQuery({
    queryKey: ['inscripciones'],
    queryFn: fetchInscripciones,
  });

  if (isLoading) return <div className='text-center p-8 text-indigo-600'>Cargando inscripciones...</div>;
  if (error) return <div className='text-center p-8 text-red-600'>Error al cargar: {error.message}</div>;

  return (
    <div className='p-6 bg-gray-50 min-h-screen'>
      <div className='max-w-4xl mx-auto'>
        <div className='flex items-center justify-between mb-6 p-4 bg-white shadow rounded-lg'>
          <h1 className='text-3xl font-bold text-indigo-700'>Inscripciones</h1>
          <div className='flex items-center space-x-4'>
            <div className='text-gray-700'>
              Usuario: <strong className='text-indigo-600'>{user?.nombreUsuario || 'N/A'}</strong>
            </div>
            <button
              onClick={handleLogout}
              className='bg-red-500 hover:bg-red-600 text-white font-semibold py-2 px-4 rounded-lg transition duration-150 shadow-md'
            >
              Cerrar Sesión
            </button>
          </div>
        </div>

        <div className='grid gap-4'>
          {inscripciones?.map(i => (
            <Link 
              key={i.id} 
              to={`/admin/inscripcion/${i.id}`} 
              className='flex justify-between items-center p-4 bg-white rounded-lg shadow-md hover:shadow-lg transition duration-200 ease-in-out hover:scale-[1.005]'
            >
              <div>
                <div className='font-medium text-gray-800'>{i.nombresApellidos}</div>
                <div className='text-sm text-gray-500'>
                  {new Date(i.fechaRegistro).toLocaleString()}
                </div>
              </div>
              <div className='text-indigo-600 font-semibold text-lg hover:text-indigo-800'>
                Ver
              </div>
            </Link>
          ))}
        </div>
        
        {(!inscripciones || inscripciones.length === 0) && !isLoading && (
            <div className='text-center p-10 bg-white rounded-lg shadow mt-4'>
                <p className='text-lg text-gray-500'>No se encontraron inscripciones.</p>
            </div>
        )}
      </div>
    </div>
  );
};

export default AdminListPage;