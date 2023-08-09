import React from 'react';
import { StyleSheet, View } from 'react-native';
import { Calendar } from 'react-native-calendars';

const CalendarComponent = ({ tasks, onDayPress, selectedDate }) => {
  // Thank god for Helper function to convert date format from F***ing "YYYY/MM/DD" to WHAT-F***ing-difference-"YYYY-MM-DD"
  const convertDateFormat = (dateString) => {
    if (!dateString) return ''; // Handle cases where dateString is undefined or null

    const [year, month, day] = dateString.split('-');
    return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
  };



  const markDates = () => {
    const markedDates = {};

    tasks.forEach((task) => {
      if (task && task.StartDate && task.Priority) {
        const { StartDate, Priority } = task;
        const convertedDate = convertDateFormat(StartDate); // Convert the date to "YYYY-MM-DD" format

        markedDates[convertedDate] = markedDates[convertedDate] || { dots: [] };

        markedDates[convertedDate].dots.push({
          key: task.Id,
          color: getColorByPriority(Priority),
        });
      }
    });

    // Add the selected date to the markedDates object
    if (selectedDate) {
      const convertedSelectedDate = selectedDate; // Convert the selected date to "YYYY-MM-DD" format
      markedDates[convertedSelectedDate] = markedDates[convertedSelectedDate] || { dots: [] };
      markedDates[convertedSelectedDate].selected = true;
      markedDates[convertedSelectedDate].selectedColor = 'lightblue';
    }

    return markedDates;
  };

  // Function to get the color based on task priority
  const getColorByPriority = (priority) => {
    switch (priority) {
      case 'low':
        return 'grey';
      case 'medium':
        return 'orange';
      case 'high':
        return 'red';
      default:
        return 'blue';
    }
  };

  return (
    <View>
      <Calendar style={styles.Calendar}
        // Minimum and maximum date that can be selected
        minDate={'2023-01-01'}
        maxDate={'2023-12-31'}
        // Date marking style [simple/period/multi-dot/custom]. Default = 'simple'
        markingType={'multi-dot'}
        // Date marking style
        markedDates={markDates()}
        // Handler for day press
        onDayPress={onDayPress}
      />
    </View>
  );
};

export default CalendarComponent;

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
  },
  Calendar: {
    elevation: 15,
    marginTop: 20,
    borderRadius: 10,
    borderColor: "lightblue",
    borderWidth: 3
  },
})