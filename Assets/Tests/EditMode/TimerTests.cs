using NUnit.Framework;

public class TimerTests
{
    public Clock clock;

    [Test]
    public void TimerActionActivatedOnTimerEnds()
    {
        bool isActivated = false;
        clock = new Clock(1);
        clock.onTimerEnd += () => isActivated = true;
        
        clock.Tick(1);
        Assert.IsTrue(isActivated);
    }

    [TestCase(-5, "00:00")]
    [TestCase(0, "00:00")]
    [TestCase(5, "00:05")]
    [TestCase(61, "01:01")]
    [TestCase(120, "02:00")]
    public void TimerFormattedCorrect(float testDuration, string expectedDuration)
    {
        clock = new Clock(testDuration);
        string value = clock.GetFormatedTime_MMSS();
        
        Assert.AreEqual(value, expectedDuration);
    }

    [Test]
    public void TimerReachedBelove0()
    {
        clock = new Clock(1);
        clock.Tick(3);
        
        Assert.AreEqual(0, clock.RemainingSeconds);
    }

    [Test]
    public void TimerActionActivatedBeforeTimerEnds()
    {
        bool isActivated = false;
        clock = new Clock(2);

        clock.onTimerEnd += () => isActivated = true;
        clock.Tick(1);
        Assert.IsFalse(isActivated);
    }
    [Test]
    public void TimerInitializedCorrectly()
    {
        float initialTime = 10f;
        clock = new Clock(initialTime);
        Assert.AreEqual(initialTime, clock.RemainingSeconds);
    }
    [Test]
    public void TimerTicksCorrectly()
    {
        float initialTime = 10f;
        float tickTime = 1f;
        clock = new Clock(initialTime);
        clock.Tick(tickTime);
        Assert.AreEqual(initialTime - tickTime, clock.RemainingSeconds);
    }
    [Test]
    public void TimerDoesNotTriggerEventAgainAfterReachingZero()
    {
        int eventTriggerCount = 0;
        clock = new Clock(1);
        clock.onTimerEnd += () => eventTriggerCount++;
        clock.Tick(1);
        clock.Tick(1);  // Tick again after timer has already reached zero
        Assert.AreEqual(1, eventTriggerCount);  // Ensure event was triggered only once
    }


}