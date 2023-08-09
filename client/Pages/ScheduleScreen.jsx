import React, { useState, useContext, useEffect } from 'react';
import { View, Text, StyleSheet, FlatList, Alert } from 'react-native';
import CalendarComponent from '../Components/CalendarComponent';
import { MainContext } from '../Components/Context/MainContextProvider';
import { Server_path2, convertDateFormat } from '../utils/api-url';

const ScheduleScreen = () => {
  const [tasks, setTasks] = useState([]);
  const [selectedDate, setSelectedDate] = useState(null);
  const { userId, convertDateFormat } = useContext(MainContext);
  const { formattedTasks, setFormattedTasks } = useContext(MainContext);

  useEffect(() => {
    loadAllTasks(userId);
  }, [userId]);

  const loadAllTasks = async (userId) => {
    try {
      const response = await fetch(`${Server_path2}/api/Tasks/GetAllTasksByUserId/${userId}`);
      if (response.ok) {
        const data = await response.json();
        setTasks(data);
      } else {
        throw new Error('Request failed');
      }
    } catch (error) {
      console.error('Error loading tasks:', error);
    }
  };


  const handleDayPress = (date) => {
    // Convert the selected date to the desired format "2023/07/01"
    const formattedDate = date.dateString;
    setSelectedDate(formattedDate);
  };


  const formatedTasks = tasks?.filter((task) => task.StartDate = task?.StartDate.substring(0, 10));
  const filteredTasks = formatedTasks?.filter((task) => task.StartDate === selectedDate);
  return (
    <View style={styles.container}>
      <CalendarComponent
        tasks={tasks}
        onDayPress={handleDayPress}
        selectedDate={selectedDate}
      />

      {selectedDate ? (
        <View style={{ flex: 1 }}>
          <FlatList
            data={filteredTasks}
            keyExtractor={(item) => item.Id.toString()}
            renderItem={({ item }) => (
              <View style={styles.taskContainer}>
                <Text style={styles.taskText}>{item.Title}</Text>
                {item.StartTime && <Text style={styles.dateText}>Start Time: {item.StartTime}</Text>}
                {item.DueTime && <Text style={styles.dateText}>Due Time: {item.DueTime}</Text>}
                <Text style={styles.priorityText}>Priority: {item.Priority}</Text>
              </View>
            )}
            ListEmptyComponent={
              <View style={styles.emptyContainer}>
                <Text style={styles.emptyText}>No tasks scheduled for {selectedDate}.</Text>
              </View>
            }
          />
        </View>
      ) : (
        <View style={styles.emptyContainer}>
          <Text style={styles.emptyText}>Select a date to view tasks.</Text>
        </View>
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
  },
  emptyContainer: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  emptyText: {
    paddingTop: 10,
    fontSize: 20,
    fontWeight: 'bold',
    color: 'gray',
  },
  taskContainer: {
    marginTop: 10,
    borderWidth: 1,
    borderBottomColor: "black",
    borderBottomWidth: 2,
    borderColor: 'gray',
    borderRadius: 8,
    padding: 12,
    marginBottom: 8,
  },
  taskText: {
    fontSize: 16,
    fontWeight: 'bold',
    marginBottom: 4,
  },
  dateText: {
    fontSize: 14,
    color: '#000000',
  },
  priorityText: {
    fontSize: 14,
    color: 'blue',
  },
});

export default ScheduleScreen;