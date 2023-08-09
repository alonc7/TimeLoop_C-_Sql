// import React, { createContext, useContext, useEffect } from 'react';
// import * as Notifications from 'expo-notifications';

// export const NotificationContext = createContext();

// function NotificationProvider({ children }) {
//     useEffect(() => {
//         const requestNotificationPermissions = async () => {
//             const { granted } = await Notifications.requestPermissionsAsync();
//             if (!granted) {
//                 console.log('Notification permissions not granted.');
//             }
//         };

//         requestNotificationPermissions();
//     }, []);

//     return (
//         <NotificationContext.Provider value={{}}>
//             {children}
//         </NotificationContext.Provider>
//     );
// };

// export const useNotificationContext = () => {
//     return useContext(NotificationContext);
// };
// export default NotificationProvider;