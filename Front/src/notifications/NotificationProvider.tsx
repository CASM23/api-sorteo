import React, { createContext, useContext, useState, ReactNode } from 'react'
import Notification, { NotificationType } from './Notification'

export interface NotificationData {
  id: string
  message: string
  type: NotificationType
  duration?: number
}

interface NotificationContextType {
  showNotification: (message: string, type: NotificationType, duration?: number) => void
}

const NotificationContext = createContext<NotificationContextType | undefined>(undefined)

export const useNotification = () => {
  const context = useContext(NotificationContext)
  if (context === undefined) {
    throw new Error('useNotification must be used within a NotificationProvider')
  }
  return context
}

interface NotificationProviderProps {
  children: ReactNode
}

export const NotificationProvider: React.FC<NotificationProviderProps> = ({ children }) => {
  const [notifications, setNotifications] = useState<NotificationData[]>([])

  const showNotification = (message: string, type: NotificationType, duration?: number) => {
    const id = Math.random().toString(36).substring(2, 9)
    const newNotification: NotificationData = { id, message, type, duration }
    
    setNotifications(prev => [...prev, newNotification])
  }

  const removeNotification = (id: string) => {
    setNotifications(prev => prev.filter(notification => notification.id !== id))
  }

  return (
    <NotificationContext.Provider value={{ showNotification }}>
      {children}
      
      {/* Contenedor de notificaciones */}
      <div className="fixed top-0 right-0 z-50 p-4 space-y-2">
        {notifications.map(notification => (
          <Notification
            key={notification.id}
            {...notification}
            onClose={removeNotification}
          />
        ))}
      </div>


    </NotificationContext.Provider>
  )
}