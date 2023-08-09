import React, { useState, useEffect, useContext } from 'react';
import { View, Text, Button, StyleSheet, Platform } from 'react-native';
import * as Device from 'expo-device';
import { getExpoPushTokenAsync, requestPermissionsAsync, setNotificationHandler } from 'expo-notifications';
import { MainContext } from './Context/MainContextProvider'

setNotificationHandler({
  handleNotification: async () => ({
    shouldShowAlert: true,
    shouldPlaySound: false,
    shouldSetBadge: false,
  }),
});

async function sendPushNotification(expoPushToken, title, body) {
  const message = {
    to: expoPushToken,
    title: title,
    body: body,
    data: { someData: 'goes here' },
  };

  await fetch('https://exp.host/--/api/v2/push/send', {
    method: 'POST',
    headers: {
      Accept: 'application/json',
      'Accept-encoding': 'gzip, deflate',
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(message),
  });
}

export default function Pnotific() {
  const [expoPushToken, setExpoPushToken] = useState('');
  const { pendingTaskList } = useContext(MainContext); // Access pendingTaskList from MainContext

  useEffect(() => {
    async function registerForPushNotifications() {
      let token;
      if (Device.isDevice) {
        const { status } = await requestPermissionsAsync();
        if (status !== 'granted') {
          alert('Failed to get push token for push notification!');
          return;
        }
        token = (await getExpoPushTokenAsync()).data;
      } else {
        alert('Must use physical device for Push Notifications');
      }

      setExpoPushToken(token);
    }

    registerForPushNotifications();
  }, []);

  useEffect(() => {
    const today = new Date();
    const tomorrow = new Date();
    tomorrow.setDate(today.getDate() + 1);

    pendingTaskList.forEach((task) => {
      const dueDate = new Date(task.dueDate);
      if (dueDate <= tomorrow) {
        sendPushNotification(expoPushToken, 'Upcoming Task', `Don't forget your task: ${task.title}`);
      }
    });
  }, [pendingTaskList, expoPushToken]);

  return (
    <View style={styles.container}>
      <Text>Your expo push token: {expoPushToken}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
});
