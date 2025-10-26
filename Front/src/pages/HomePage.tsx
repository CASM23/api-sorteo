import { useNavigate } from 'react-router-dom';

const HomePage = () => {
    const nav = useNavigate();
    
    return (
        <div className='flex items-center justify-center min-h-screen bg-indigo-700 p-4'>
            <div className='bg-white shadow-2xl rounded-xl w-full max-w-2xl p-8 text-center'>
                <h1 className='text-4xl sm:text-5xl font-extrabold text-indigo-800 mb-4'>
                    ¡Gran Sorteo SYC!
                </h1>
                <p className='text-xl text-gray-600 mb-8'>
                    Regístrate para tener la oportunidad de ganar fantásticos premios.
                </p>

                <div className='flex flex-col sm:flex-row gap-6 justify-center'>
                    <button 
                        onClick={() => nav('/inscripcion')}
                        className='flex-1 px-8 py-4 bg-green-500 hover:bg-green-600 text-white font-bold text-lg rounded-xl shadow-lg transition duration-300 transform hover:scale-[1.03] hover:shadow-xl'
                    >
                        Inscripción al Sorteo
                    </button>
                    <button 
                        onClick={() => nav('/login')}
                        className='flex-1 px-8 py-4 bg-gray-200 hover:bg-gray-300 text-indigo-700 font-bold text-lg rounded-xl shadow-lg transition duration-300 transform hover:scale-[1.03] hover:shadow-xl'
                    >
                        Acceso Administrador
                    </button>
                </div>
                
                <p className='mt-8 text-sm text-gray-500'>
                    ¡Tu oportunidad te espera! Inscripciones abiertas para mayores de edad.
                </p>
            </div>
        </div>
    );
};

export default HomePage;
